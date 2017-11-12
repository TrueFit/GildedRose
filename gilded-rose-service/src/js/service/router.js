/**
 * Service implementation, this is where we will deal with the actual request.
 */
import express from 'express';
import bodyParser from 'body-parser';
import cors from 'cors';
const compression = require('compression');
import _ from 'lodash';
import awsServerlessExpressMiddleware from 'aws-serverless-express/middleware';

// Internal libraries
import * as service from './serviceImpl';
// Constants
const BASE_PATH = '/gilded-rose';

// Initialize Express
const app = express();
app.set('view engine', 'pug');
// app.use(compression());
app.use(cors());
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(awsServerlessExpressMiddleware.eventContext());

// Define routing

// Get all items (current inventory)
app.get('/items', (req, res) => {
  console.log("Handling request to list all items in inventory");
  service.listItems().then((itemList) => {
    console.log("Got item list", itemList, _.isArray(itemList));
    res.json(itemList);
  }).catch((err) => {
    res.status(500).json(err);
  });
  console.log("Exit function");
});

app.get('/items/nextDay', (req, res) => {
  console.log("Handling simple GET request", req);
  res.json({op:'xyz'});
});

// Post on or more additional items to add to the inventory
// app.post('/items?', (req, res) => {
//   console.log("Handling simple GET request", req);
//   res.json(req.params);
// });

// Get a specific item by id
app.get('/item/:itemId', (req, res) => {
  console.log("Handling simple GET request", req);
  res.json(req.params);
});

// Delete a specific item by id
app.delete('/item/:itemId', (req, res) => {
  console.log("Handling simple GET request", req);
  res.json(req.params);
});





export default app;