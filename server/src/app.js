const express = require('express');
const inventory = require('./inventory');

const app = express();

app.get('/items/:id', (req, res) => {
  inventory.find().then(items => {
    res.send('item details');
  });
});

app.get('/items', (req, res) => {
  inventory.find().then(items => {
    res.send('item list');
  });
});

app.patch('/items', (req, res) => {
  inventory.nextDay().then(items => {
    res.send('progress to next day');
  });
});

module.exports = app;
