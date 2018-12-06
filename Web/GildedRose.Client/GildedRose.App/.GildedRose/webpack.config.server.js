//var WebpackDevServer = require('webpack-dev-server');
var webpackSettings = require('./webpack.config');
webpackSettings.devtool = 'eval';

webpackSettings.devServer = {
  overlay: true,
  historyApiFallback: {
    index: '/gildedrose/'
  },
  proxy: {
    '/api/*': {
      target: 'http://localhost:5000',
      ws: false,
      secure: false,
      changeOrigin: true,
    },
    '/swagger/*': {
      target: 'http://localhost:5000',
      ws: false,
      secure: false,
      changeOrigin: true,
    }
  }
}

console.log('Skipping proxy for browser request.');

module.exports = webpackSettings;
