const express = require('express');
const inventory = require('./inventory');

const app = express();

// "Ask for the details of a single item by name"
app.get('/items/:name', (req, res) => {
  inventory.find({name: req.params.name}).then(item => {
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
