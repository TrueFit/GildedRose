module.exports =
/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


/**
 * Lambda Handler, entry point.
 * This will delegate handling of requests to the Express 4 Middleware.
 */

var awsServerlessExpress = __webpack_require__(1);
var app = __webpack_require__(5);
var server = awsServerlessExpress.createServer(app);

exports.handler = function (event, context) {
  return awsServerlessExpress.proxy(server, event, context);
};

/***/ }),
/* 1 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/*
 * Copyright 2016-2016 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *     http://aws.amazon.com/apache2.0/
 *
 * or in the "license" file accompanying this file.
 * This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

const http = __webpack_require__(2)
const url = __webpack_require__(3)
const binarycase = __webpack_require__(4)

function getPathWithQueryStringParams(event) {
  return url.format({ pathname: event.path, query: event.queryStringParameters })
}

function getContentType(params) {
  // only compare mime type; ignore encoding part
  return params.contentTypeHeader ? params.contentTypeHeader.split(';')[0] : ''
}

function isContentTypeBinaryMimeType(params) {
  return params.binaryMimeTypes.indexOf(params.contentType) !== -1
}

function mapApiGatewayEventToHttpRequest(event, context, socketPath) {
    const headers = event.headers || {} // NOTE: Mutating event.headers; prefer deep clone of event.headers
    const eventWithoutBody = Object.assign({}, event)
    delete eventWithoutBody.body

    headers['x-apigateway-event'] = encodeURIComponent(JSON.stringify(eventWithoutBody))
    headers['x-apigateway-context'] = encodeURIComponent(JSON.stringify(context))

    return {
        method: event.httpMethod,
        path: getPathWithQueryStringParams(event),
        headers,
        socketPath
        // protocol: `${headers['X-Forwarded-Proto']}:`,
        // host: headers.Host,
        // hostname: headers.Host, // Alias for host
        // port: headers['X-Forwarded-Port']
    }
}

function forwardResponseToApiGateway(server, response, context) {
    let buf = []

    response
        .on('data', (chunk) => buf.push(chunk))
        .on('end', () => {
            const bodyBuffer = Buffer.concat(buf)
            const statusCode = response.statusCode
            const headers = response.headers

            // chunked transfer not currently supported by API Gateway
            if (headers['transfer-encoding'] === 'chunked') delete headers['transfer-encoding']

            // HACK: modifies header casing to get around API Gateway's limitation of not allowing multiple
            // headers with the same name, as discussed on the AWS Forum https://forums.aws.amazon.com/message.jspa?messageID=725953#725953
            Object.keys(headers)
                .forEach(h => {
                    if(Array.isArray(headers[h])) {
                      if (h.toLowerCase() === 'set-cookie') {
                        headers[h].forEach((value, i) => {
                          headers[binarycase(h, i + 1)] = value
                        })
                        delete headers[h]
                      } else {
                        headers[h] = headers[h].join(',')
                      }
                    }
                })

            const contentType = getContentType({ contentTypeHeader: headers['content-type'] })
            const isBase64Encoded = isContentTypeBinaryMimeType({ contentType, binaryMimeTypes: server._binaryTypes })
            const body = bodyBuffer.toString(isBase64Encoded ? 'base64' : 'utf8')
            const successResponse = {statusCode, body, headers, isBase64Encoded}

            context.succeed(successResponse)
        })
}

function forwardConnectionErrorResponseToApiGateway(server, error, context) {
    console.log('ERROR: aws-serverless-express connection error')
    console.error(error)
    const errorResponse = {
        statusCode: 502, // "DNS resolution, TCP level errors, or actual HTTP parse errors" - https://nodejs.org/api/http.html#http_http_request_options_callback
        body: '',
        headers: {}
    }

    context.succeed(errorResponse)
}

function forwardLibraryErrorResponseToApiGateway(server, error, context) {
    console.log('ERROR: aws-serverless-express error')
    console.error(error)
    const errorResponse = {
        statusCode: 500,
        body: '',
        headers: {}
    }

    context.succeed(errorResponse)
}

function forwardRequestToNodeServer(server, event, context) {
    try {
        const requestOptions = mapApiGatewayEventToHttpRequest(event, context, getSocketPath(server._socketPathSuffix))
        const req = http.request(requestOptions, (response, body) => forwardResponseToApiGateway(server, response, context))
        if (event.body) {
            if (event.isBase64Encoded) {
                event.body = new Buffer(event.body, 'base64')
            }

            req.write(event.body)
        }

        req.on('error', (error) => forwardConnectionErrorResponseToApiGateway(server, error, context))
        .end()
    } catch (error) {
       forwardLibraryErrorResponseToApiGateway(server, error, context)
       return server
   }
}

function startServer(server) {
    return server.listen(getSocketPath(server._socketPathSuffix))
}

function getSocketPath(socketPathSuffix) {
    return `/tmp/server${socketPathSuffix}.sock`
}

function createServer (requestListener, serverListenCallback, binaryTypes) {
    const server = http.createServer(requestListener)

    server._socketPathSuffix = 0
    server._binaryTypes = binaryTypes ? binaryTypes.slice() : []
    server.on('listening', () => {
        server._isListening = true

        if (serverListenCallback) serverListenCallback()
    })
    server.on('close', () => {
        server._isListening = false
    })
    .on('error', (error) => {
        if (error.code === 'EADDRINUSE') {
            console.warn(`WARNING: Attempting to listen on socket ${getSocketPath(server._socketPathSuffix)}, but it is already in use. This is likely as a result of a previous invocation error or timeout. Check the logs for the invocation(s) immediately prior to this for root cause, and consider increasing the timeout and/or cpu/memory allocation if this is purely as a result of a timeout. aws-serverless-express will restart the Node.js server listening on a new port and continue with this request.`)
            ++server._socketPathSuffix
            return server.close(() => startServer(server))
        }

        console.log('ERROR: server error')
        console.error(error)
    })

    return server
}

function proxy(server, event, context) {
    if (server._isListening) {
      forwardRequestToNodeServer(server, event, context)
      return server
    } else {
        return startServer(server)
        .on('listening', () => proxy(server, event, context))
    }
}

exports.createServer = createServer
exports.proxy = proxy

if (process.env.NODE_ENV === 'test') {
    exports.getPathWithQueryStringParams = getPathWithQueryStringParams
    exports.mapApiGatewayEventToHttpRequest = mapApiGatewayEventToHttpRequest
    exports.forwardResponseToApiGateway = forwardResponseToApiGateway
    exports.forwardConnectionErrorResponseToApiGateway = forwardConnectionErrorResponseToApiGateway
    exports.forwardLibraryErrorResponseToApiGateway = forwardLibraryErrorResponseToApiGateway
    exports.forwardRequestToNodeServer = forwardRequestToNodeServer
    exports.startServer = startServer
    exports.getSocketPath = getSocketPath
}


/***/ }),
/* 2 */
/***/ (function(module, exports) {

module.exports = require("http");

/***/ }),
/* 3 */
/***/ (function(module, exports) {

module.exports = require("url");

/***/ }),
/* 4 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 *  @license
 *    Copyright 2016 Brigham Young University
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 **/


/**
 * Toggle the case of a string based on the number value passed in.
 * @param {string} string
 * @param {number} number
 * @param {object} [options={allowOverflow: true}]
 * @returns {string|boolean}
 */
module.exports = binaryCase;

function binaryCase(string, number, options) {
    if (!options || typeof options !== 'object') options = {};
    if (!options.hasOwnProperty('allowOverflow')) options.allowOverflow = true;

    if (number > binaryCase.maxNumber(string) && !options.allowOverflow) return false;

    return getBinaryCase(string, number);
}

binaryCase.iterator = function(string, options) {
    const max = binaryCase.maxNumber(string);

    if (!options || typeof options !== 'object') options = {};
    if (!options.hasOwnProperty('startIndex')) options.startIndex = 0;
    if (typeof options.startIndex !== 'number' || !Number.isInteger(options.startIndex) || options.startIndex < 0) throw Error('Option startIndex must be a non-negative integer.');

    var index = options.startIndex;
    return {
        next: function() {
            return index > max
                ? { done: true }
                : { done: false, value: getBinaryCase(string, index++) };
        }
    };
};

/**
 * Get the maximum number that can be used before causing overflow.
 * @param {string} string
 * @returns {number}
 */
binaryCase.maxNumber = function(string) {
    const pow = string.match(/[a-z]/ig).length;
    return Math.pow(2, pow) - 1;
};

/**
 * Get an array of all possible variations.
 * @param {string} string
 * @returns {string[]}
 */
binaryCase.variations = function(string) {
    const results = [];
    const max = binaryCase.maxNumber(string);
    for (var i = 0; i <= max; i++) {
        results.push(binaryCase(string, i));
    }
    return results;
};


function getBinaryCase(string, number) {
    const binary = (number >>> 0).toString(2);

    var bin;
    var ch;
    var i;
    var j = binary.length - 1;
    var offset;
    var result = '';
    for (i = 0; i < string.length; i++) {
        ch = string.charAt(i);
        if (/[a-z]/ig.test(ch)) {
            bin = binary.charAt(j--);

            if (bin === '1') {
                offset = ch >= 'a' && ch <= 'z' ? -32 : 32;
                result += String.fromCharCode(ch.charCodeAt(0) + offset);
            } else {
                result += ch;
            }

            if (j < 0) {
                result += string.substr(i + 1);
                break;
            }
        } else {
            result += ch;
        }
    }
    return result;
}

/***/ }),
/* 5 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


/**
 * Service implementation, this is where we will deal with the actual request.
 */

var awsServerlessExpressMiddleware = __webpack_require__(6);
app.use(awsServerlessExpressMiddleware.eventContext());
app.get('/', function (req, res) {
  res.json(req.apiGateway.event);
});

/***/ }),
/* 6 */
/***/ (function(module, exports) {

module.exports.eventContext = options => (req, res, next) => {
    options = options || {} // defaults: {reqPropKey: 'apiGateway', deleteHeaders: true}
    const reqPropKey = options.reqPropKey || 'apiGateway'
    const deleteHeaders = options.deleteHeaders === undefined ? true : options.deleteHeaders

    if (!req.headers['x-apigateway-event'] || !req.headers['x-apigateway-context']) {
        console.error('Missing x-apigateway-event or x-apigateway-context header(s)')
        next()
        return
    }

    req[reqPropKey] = {
        event: JSON.parse(decodeURIComponent(req.headers['x-apigateway-event'])),
        context: JSON.parse(decodeURIComponent(req.headers['x-apigateway-context']))
    }

    if (deleteHeaders) {
        delete req.headers['x-apigateway-event']
        delete req.headers['x-apigateway-context']
    }

    next()
}


/***/ })
/******/ ]);