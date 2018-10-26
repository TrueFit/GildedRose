const express = require('express');
const app = express();

app.get('/items/:id', (req, res) => {
  res.send('item details');
});

app.get('/items', (req, res) => {
  res.send('item list');
});

app.patch('/items', (req, res) => {
  res.send('progress to next day');
});

module.exports = app;
