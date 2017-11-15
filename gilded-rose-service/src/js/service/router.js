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
app.get('/inventory', (req, res) => {
  service.listItems().then((itemList) => {
    res.json(itemList);
  }).catch((err) => {
    res.status(500).json(err);
  });
});

app.get('/inventory/reset', (req, res) => {
  service.resetInventory().then((itemList) => {
    res.json(itemList);
  }).catch((err) => {
    res.status(500).json(err);
  });
});

// Get the inventory for the next day, move time forward
app.get('/inventory/nextDay', (req, res) => {
  service.nextDay().then((result)=>{
    res.json(result);
  }).catch((err)=>{
    res.status(500).json(err);
  });
});

// Post one or more additional items to add to the inventory
app.post('/items?', (req, res) => {

  const func = (_.isArray(req.body)) ? service.addItems : service.addItem;

  func(req.body).then((result)=>{
    res.json(result);
  }).catch((err)=>{
    res.status(500).json(err);
  });
});

// Get a specific item by id
app.get('/item/:itemId', (req, res) => {
  service.deleteItem(req.params.itemId).then((result)=>{
    res.json(result);
  }).catch((err)=>{
    res.status(500).json(err);
  });
});

// Delete a specific item by id
app.delete('/item/:itemId', (req, res) => {
  service.deleteItem(req.params.itemId).then((result)=>{
    res.json(result);
  }).catch((err)=>{
    res.status(500).json(err);
  });
});

export default app;