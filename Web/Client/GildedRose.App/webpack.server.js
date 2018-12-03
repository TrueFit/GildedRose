var WebpackDevServer = require('webpack-dev-server');

var webpackSettings = require('./webpack.config');
webpackSettings.devtool = 'eval';

webpackSettings.devServer = {
    overlay: true,
   historyApiFallback:{
            index:'/gildedrose/'
        },

    proxy: {
        '/api/*': {
            target: 'http://localhost',
            ws: false,
            secure: false,
            changeOrigin: true,
        }
    }
}

console.log('Skipping proxy for browser request.');

module.exports = webpackSettings;
