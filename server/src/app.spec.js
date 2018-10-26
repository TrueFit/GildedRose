const request = require('supertest');
const expect = require('expect.js');
const app = require('./app');

describe('server', () => {
  it('shows a message', () => {
    return request(app)
      .get('/')
      .expect(200)
      .then(res => {
        expect(res.text).to.equal('Gilded Rose');
      });
  });
});
