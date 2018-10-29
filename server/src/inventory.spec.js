const inventory = require('./inventory');

describe('inventory', () => {
  beforeEach(() => inventory.init());

  it('finds all', async () => {
    const items = await inventory.find();
    expect(items).toHaveLength(20);
  });

  it('finds items matching a query', async () => {
    const items = await inventory.find({quality: 10});
    expect(items).toHaveLength(5);
  });

  it('advances to the next day', async () => {
    const [initial] = await inventory.find();
    const expected = {
      ...initial,
      quality: initial.quality - 1,
      sellIn: initial.sellIn - 1,
    };
    const [actual] = await inventory.nextDay();
    expect(actual).toEqual(expected);
  });
});
