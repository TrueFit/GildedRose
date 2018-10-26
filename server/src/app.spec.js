const request = require('supertest');
const app = require('./app');
const inventory = require('./inventory');

jest.mock('./inventory');

describe('server', () => {
  beforeEach(() => {
    inventory.find.mockReturnValue(Promise.resolve([]));
    inventory.nextDay.mockReturnValue(Promise.resolve([]));
  });

  it('returns the entire list of inventory', () => {
    return request(app)
      .get('/items')
      .expect(200)
      .then(res => {
        expect(inventory.find).toBeCalled();
      });
  });

  it('returns details of a single item', () =>
    request(app)
      .get('/items/1')
      .expect(200));

  it('progresses to the next day', () =>
    request(app)
      .patch('/items')
      .expect(200));

  it('returns trash', () =>
    request(app)
      .get('/items?quality=0')
      .expect(200));
});
