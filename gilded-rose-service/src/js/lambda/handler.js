/**
 * Lambda Handler, entry point.
 * This will delegate handling of requests to the Express 4 Middleware.
 */

const awsServerlessExpress = require('aws-serverless-express')
const app = require('../service/app')
const server = awsServerlessExpress.createServer(app)

exports.handler = (event, context) => awsServerlessExpress.proxy(server, event, context)