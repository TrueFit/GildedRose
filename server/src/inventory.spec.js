const inventory = require('./inventory');

describe('inventory', () => {
  beforeEach(() => inventory.init());

  it('finds all', () => {
    return inventory.find().then(items => {
      expect(items).toHaveLength(20);
    });
  });

  it('finds items matching a query', () => {
    return inventory.find({ quality: 10 }).then(items => {
      expect(items).toHaveLength(5);
    });
  });
});
