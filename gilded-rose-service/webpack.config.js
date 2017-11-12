const webpack = require('webpack'); 
const path = require('path');
const LodashModuleReplacementPlugin = require('lodash-webpack-plugin');

const outputPath = path.resolve(__dirname,"./dist")

module.exports = {
    entry: "./src/js/lambda/handler.js",
    target: 'node',
    output: {
        path: outputPath,
        filename: "index.js",
        library: 'handler',
        libraryTarget: 'commonjs2'
    },
    module: {
        loaders: [
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                loader: 'babel-loader',
                query: {
                    presets: ['es2015']
                }
            }
        ]
    },
    plugins: [
        new LodashModuleReplacementPlugin,
        new webpack.optimize.OccurrenceOrderPlugin,
        new webpack.optimize.UglifyJsPlugin()
    ]
};