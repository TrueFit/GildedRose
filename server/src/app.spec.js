const request = require('supertest');
const app = require('./app');
const inventory = require('./inventory');

jest.mock('./inventory');

describe('server', () => {
  let item;

  beforeEach(() => {
    item = {
      name: 'test',
      category: 'category',
      sellIn: 5,
      quality: 10,
    };
  });

  it('returns the entire list of inventory', async () => {
    inventory.find.mockReturnValue(Promise.resolve([item]));
    await request(app)
      .get('/items')
      .expect(200)
      .expect([item]);
    expect(inventory.find).toBeCalled();
  });

  describe('details for an item by name', () => {
    it('returns details of a single item', async () => {
      inventory.find.mockReturnValue(Promise.resolve(item));
      await request(app)
        .get('/items/name')
        .expect(200)
        .expect(item);
      expect(inventory.find).toBeCalledWith({name: 'name'});
    });

    it('404s if item is not found', async () => {
      inventory.find.mockReturnValue(Promise.resolve(undefined));
      await request(app)
        .get('/items/name')
        .expect(404);
      expect(inventory.find).toBeCalledWith({name: 'name'});
    });
  });

  it('progresses to the next day', async () => {
    inventory.nextDay.mockReturnValue(Promise.resolve([item]));
    await request(app)
      .patch('/items')
      .expect(200)
      .expect([item]);
    expect(inventory.nextDay).toBeCalled();
  });

  it('returns trash', async () => {
    inventory.find.mockReturnValue(Promise.resolve(item));
    await request(app)
      .get('/items?quality=0')
      .expect(200)
      .expect(item);
    expect(inventory.find).toBeCalledWith({quality: 0});
  });
});
