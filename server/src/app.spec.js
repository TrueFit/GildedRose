const request = require('supertest');
const expect = require('expect.js');
const app = require('./app');

describe('server', () => {
  it('returns the entire list of inventory', () =>
    request(app)
      .get('/items')
      .expect(200));

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
