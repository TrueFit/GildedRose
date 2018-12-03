var webpack = require('webpack');

module.exports = {
    entry: {
        'vendor': [
            "react",
            "react-dom",
            "react-router",
            "redux",
            "react-router-redux",
            "react-redux",
            "redux-logger",
            "redux-thunk",
            "redux-saga",
            "redux-form",
            "moment",
            "js-cookie",
            "axios",
            "lodash",
            "chart.js",
            "loglevel",
            "react-chartjs"
        ],
    },
    devtool: 'source-map',
    output: {
        filename: "[name].bundle.js",
        path: __dirname + "/dist",
        library: "[name]_lib"
    },
    plugins: [
        new webpack.DllPlugin({
            // The path to the manifest file which maps between
            // modules included in a bundle and the internal IDs
            // within that bundle
            path: 'dist/[name]-manifest.json',
            // The name of the global variable which the library's
            // require function has been assigned to. This must match the
            // output.library option above
            name: '[name]_lib'
        }),
        new webpack.ContextReplacementPlugin(/moment[\/\\]locale$/, /9/)
    ]
}