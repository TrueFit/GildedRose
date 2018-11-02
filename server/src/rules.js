// constants
const dailyQualityChange = 1;
const minQuality = 0;
const maxQuality = 50;

// helpers
const nextSellIn = i => ({...i, sellIn: i.sellIn - 1});
const clipQuality = quality =>
  Math.max(minQuality, Math.min(maxQuality, quality));

/**
 * Rules are objects with the following functions:
 *
 *   `test` - checks if the rule applies to the item
 *
 *   `nextSellIn` - returns a copy of the item with the `sellIn` value updated
 *      for the next day
 *
 *   `nextQuality` - returns a copy of the item with the `quality` value updated
 *      for the next day
 * 
 * This should make it easy to add and change edge cases!
 */
const rules = [
  {
    test: i => i.name === 'Aged Brie',
    nextSellIn,
    nextQuality: i => ({
      ...i,
      quality: clipQuality(i.quality + dailyQualityChange),
    }),
  },

  {
    test: i => i.category === 'Sulfuras',
    nextSellIn: i => ({...i}),
    nextQuality: i => ({...i}),
  },

  {
    test: i =>
      // accounting for a typo in the data :)
      i.category === 'Backstage Passes' || i.category === 'Backstage Pasess',
    nextSellIn,
    nextQuality: i => {
      if (i.sellIn < 0) {
        // assumes that a sellIn of 0 means that this is the day of the
        // concert, which means quality should not be 0 yet
        return {...i, quality: 0};
      } else if (i.sellIn <= 5) {
        return {...i, quality: clipQuality(i.quality + 3)};
      } else if (i.sellIn <= 10) {
        return {...i, quality: clipQuality(i.quality + 2)};
      } else {
        return {...i, quality: clipQuality(i.quality + 1)};
      }
    },
  },

  {
    test: i => i.category === 'Conjured',
    nextSellIn,
    nextQuality: i => ({
      ...i,
      quality: clipQuality(i.quality - 2 * dailyQualityChange),
    }),
  },

  {
    // default rule. this works because find() returns the first match
    test: i => true,
    nextSellIn,
    nextQuality: i => {
      const change = i.sellIn < 0 ? 2 * dailyQualityChange : dailyQualityChange;
      return {
        ...i,
        quality: clipQuality(i.quality - change),
      };
    },
  },
];

// gets the first rule that matches the item
function getRule(item) {
  return rules.find(r => r.test(item));
}

module.exports = {getRule};
