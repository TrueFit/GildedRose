/**
 * Lambda Handler, entry point.
 * This will delegate handling of requests to the Express 4 Middleware.
 */

import awsServerlessExpress from 'aws-serverless-express';
import router from '../service/router';
const server = awsServerlessExpress.createServer(router);

exports.handler = (event, context) => awsServerlessExpress.proxy(server, event, context)