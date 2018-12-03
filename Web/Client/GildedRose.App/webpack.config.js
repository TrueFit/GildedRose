var HtmlWebpackPlugin = require('html-webpack-plugin');
var webpack = require("webpack");
const AddAssetHtmlPlugin = require('add-asset-html-webpack-plugin');
var path = require("path");
var currentDirectory = __dirname;

var webpackSettings = {
    cache: true,
    entry: "./src/app.tsx",
    devtool: "eval",
    output: {
        path: __dirname + "/dist",
        filename: '[name].[hash].js',
        publicPath: "/gildedrose/",
        chunkFilename: "[name].[hash].js"
    },
    resolve: {
        extensions: ['.webpack.js', '.web.js', '.ts', '.tsx', '.js'],
        alias: {
            'models': currentDirectory + "/src/models/",
            'core': path.resolve("../Core/src/"),
            'coremodels': path.resolve("../Core/src/models/")
        }
    },
    module: {
        rules: [
            {
                test: /\.(tsx|ts)$/,
                enforce: 'pre',
                loader: 'tslint-loader',
                options: {
                    emitErrors: true,
                    failOnHint: true
                }
            },
            {
                test: /\.(tsx|ts)$/,
                use: "ts-loader"
            },
            {
                test: /\.(png|gif|jpg|woff|eot|ttf|svg|woff2)$/i,
                use: "file-loader?name=content/[name][hash].[ext]"
            },
            {
                test: /\.(config)$/i,
                use: "file-loader?name=[name].[ext]"
            },
            {
                test: /\.css$/,
                use: ["style-loader", "css-loader", "postcss-loader"]
            }
        ]
    },
    plugins: [
        new HtmlWebpackPlugin({
            template: 'src/index.html',
            filename: 'index.html'
        }),
        new webpack.DllReferencePlugin({
            context: process.cwd(),
            manifest: require("./dist/vendor-manifest.json")
        }),
        new AddAssetHtmlPlugin({ 
            filepath: require.resolve('./dist/vendor.bundle.js')
        })
    ]
}

module.exports = webpackSettings;
