// default
const nextSellIn = i => ({...i, sellIn: i.sellIn - 1});

const rules = [
  {
    test: i => i.name === 'Aged Brie',
    nextSellIn,
    nextQuality: i => ({...i, quality: Math.min(50, i.quality + 1)}),
  },

  {
    test: i => i.category === 'Sulfuras',
    nextSellIn: i => ({...i}),
    nextQuality: i => ({...i}),
  },

  {
    test: i => i.category === 'Backstage Passes',
    nextSellIn,
    nextQuality: i => {
      if (i.sellIn < 0) {
        // assumes that a sellIn of 0 means that this is the day of the
        // concert, which means quality should not be 0 yet
        return {...i, quality: 0};
      } else if (i.sellIn <= 5) {
        return {...i, quality: i.quality + 3};
      } else if (i.sellIn <= 10) {
        return {...i, quality: i.quality + 2};
      } else {
        return {...i, quality: i.quality + 1};
      }
    },
  },

  {
    test: i => i.category === 'Conjured',
    nextSellIn,
    nextQuality: i => ({...i, quality: Math.max(0, i.quality - 2)}),
  },

  {
    // default rule. this works because find() returns the first match
    test: i => true,
    nextSellIn,
    nextQuality: i => ({...i, quality: Math.max(0, i.quality - 1)}),
  },
];

function getRule(item) {
  return rules.find(r => r.test(item));
}

module.exports = {getRule};
