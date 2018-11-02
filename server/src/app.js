const express = require('express');
const inventory = require('./inventory');

const app = express();

// enabling cors
// we'd want to make sure this is set up correctly to our
// production deployment environment
app.use((req, res, next) => {
  res.header("Access-Control-Allow-Origin", "*");
  res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
  res.header('Access-Control-Allow-Methods', 'GET, PATCH');
  next();
});

// "Ask for the details of a single item by name"
// (I'd probably recommend doing this by ID instead of name.)
app.get('/items/:name', (req, res) => {
  inventory.get(req.params.name).then(item => {
    if (item) {
      res.send(item);
    } else {
      res.send(404);
    }
  });
});

// "Ask for the entire list of inventory"
// "List of trash we should throw away (Quality = 0)"
app.get('/items', (req, res) => {
  const query = parseQuery(req.query);
  inventory.find(query).then(items => {
    res.send(items);
  });
});

// "Progress to the next day"
// using PATCH because it's not idempotent and can be used for partial changes.
// PUT wouldn't work because it only allows for replacing the document.
app.patch('/items', (req, res) => {
  inventory.nextDay().then(items => {
    res.send(items);
  });
});

// If this gets any longer, it would be worth moving it to its own file and
// building out more tests, but for now, it's covered by other tests.
function parseQuery(query) {
  if (query === undefined) {
    return undefined;
  }

  let parsed = {...query};

  if (parsed.quality !== undefined) {
    parsed.quality = parseInt(parsed.quality, 10);
  }

  if (parsed.sellIn !== undefined) {
    parsed.sellIn = parseInt(parsed.sellIn, 10);
  }

  return parsed;
}
module.exports = app;
