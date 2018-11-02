// TODO note about using fs instead of a real database
const fs = require('fs');
const rules = require('./rules');

// TODO note about storing changes in memory
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

// TODO note that this is a very simple query inerface
async function find(query = {}) {
  return items.filter(i => matches(i, query));
}

async function nextDay() {
  // storing in memory, but this is where we'd persist the changes
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
