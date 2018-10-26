const rules = require('./rules');
const {getRule} = rules;

describe('inventory rules', () => {
  let item;

  describe('default', () => {
    beforeEach(() => {
      item = {
        name: 'test',
        category: 'category',
        sellIn: 5,
        quality: 10,
      };
    });

    it('decreases sellIn', () => {
      const expected = {...item, sellIn: 4};
      const actual = getRule(item).nextSellIn(item);
      expect(actual).toEqual(expected);
    });

    describe('quality', () => {
      it('decreases', () => {
        const expected = {...item, quality: 9};
        const actual = getRule(item).nextQuality(item);
        expect(actual).toEqual(expected);
      });

      it('is never negative', () => {
        item = {...item, quality: 0};
        const actual = getRule(item).nextQuality(item);
        expect(actual).toEqual(item);
      });
    });
  });

  describe('aged brie', () => {
    beforeEach(() => {
      item = {
        name: 'Aged Brie',
        category: 'Food',
        sellIn: 50,
        quality: 10,
      };
    });

    it('increases in quality', () => {
      const expected = {...item, quality: 11};
      const actual = getRule(item).nextQuality(item);
      expect(actual).toEqual(expected);
    });

    it('...but not more than 50', () => {
      item = {...item, quality: 50};
      const actual = getRule(item).nextQuality(item);
      expect(actual).toEqual(item);
    });
  });

  describe('sulfuras', () => {
    beforeEach(() => {
      item = {
        name: 'Hand of Ragnaros',
        category: 'Sulfuras',
        sellIn: 80,
        quality: 80,
      };
    });

    it('never decrements sellIn', () => {
      const actual = getRule(item).nextSellIn(item);
      expect(actual).toEqual(item);
    });

    it('never decrements quality', () => {
      const actual = getRule(item).nextQuality(item);
      expect(actual).toEqual(item);
    });
  });

  describe('backstage passes', () => {
    beforeEach(() => {
      item = {
        name: 'I am Murloc',
        category: 'Backstage Passes',
        sellIn: 20,
        quality: 10,
      };
    });

    it('increases quality by 1 when more than 10 days away', () => {
      const expected = {
        ...item,
        sellIn: 20,
        quality: item.quality + 1,
      };
      const actual = getRule(item).nextQuality(item);
      expect(actual).toEqual(expected);
    });

    it('increases quality by 2 when <= 10 days away', () => {
      item = {...item, sellIn: 10};
      const expected = {
        ...item,
        quality: item.quality + 2,
      };
      const actual = getRule(item).nextQuality(item);
      expect(actual).toEqual(expected);
    });

    it('increases quality by 3 when <= 5 days away', () => {
      item = {...item, sellIn: 5};
      const expected = {
        ...item,
        quality: item.quality + 3,
      };
      const actual = getRule(item).nextQuality(item);
      expect(actual).toEqual(expected);
    });

    it('sets quality to 0 after the concert', () => {
      item = {...item, sellIn: -1};
      const expected = {
        ...item,
        sellIn: -1,
        quality: 0,
      };
      const actual = getRule(item).nextQuality(item);
      expect(actual).toEqual(expected);
    });
  });

  describe('conjured', () => {
    beforeEach(() => {
      item = {
        name: 'Giant Slayer',
        category: 'Conjured',
        sellIn: 15,
        quality: 50,
      };
    });

    it('degrades in quality twice as fast', () => {
      const expected = {...item, quality: 48};
      const actual = getRule(item).nextQuality(item);
      expect(actual).toEqual(expected);
    });

    it('...but still not less than 0', () => {
      item = {...item, quality: 0};
      const actual = getRule(item).nextQuality(item);
      expect(actual).toEqual(item);
    });
  });
});
