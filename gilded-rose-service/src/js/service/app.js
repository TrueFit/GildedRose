/**
 * Service implementation, this is where we will deal with the actual request.
 */

const awsServerlessExpressMiddleware = require('aws-serverless-express/middleware')
app.use(awsServerlessExpressMiddleware.eventContext())
app.get('/', (req, res) => {
  res.json(req.apiGateway.event)
})