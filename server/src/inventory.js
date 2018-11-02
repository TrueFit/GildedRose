// I decided to read the data from the file and store changes
// in memory. This allows for an easier setup, and it makes it
// very easy to reset the data during review.
//
// I wouldn't do this for a production application though! It would
// certainly require a database. In this case, I'd probably recommend
// Mongo because the data is document-oriented and has no relations.
//
// If we were going to update this file to use a database, the
// exported function signatures could remain the same, but the implementations
// would be different.
const fs = require('fs');
const rules = require('./rules');

let items = undefined;

// TODO refactor to async/await
function init() {
  return new Promise((resolve, reject) => {
    fs.readFile('../inventory.txt', 'utf8', (err, raw) => {
      err ? reject(err) : resolve(raw);
    });
  })
    .then(raw => raw.split(/\n/g))
    .then(lines => lines.filter(line => line !== ''))
    .then(lines =>
      lines.map(line => {
        const [name, category, sellIn, quality] = line.split(',');
        return {
          name,
          category,
          sellIn: parseInt(sellIn, 10),
          quality: parseInt(quality, 10),
        };
      }),
    )
    .then(parsed => (items = parsed));
}

// This is a very simple query interface that only returns
// items where the key matches the value. A full database would
// give us a more complete query interface.
async function find(query = {}) {
  return items.filter(i => matches(i, query));
}

async function nextDay() {
  // Storing in memory, but this is where we'd persist the changes.
  items = items.map(nextDayItem);
  return items;
}

async function get(name) {
  return items.find(i => i.name == name);
}

function matches(item, query) {
  const keys = Object.keys(query);
  return keys.reduce((acc, key) => acc && item[key] === query[key], true);
}

function nextDayItem(item) {
  const rule = rules.getRule(item);
  return rule.nextQuality(rule.nextSellIn(item));
}

module.exports = {find, get, init, nextDay};
