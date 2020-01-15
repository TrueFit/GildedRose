const { CleanWebpackPlugin } = require('clean-webpack-plugin');

applications = {
    entry: {
        inventory: __dirname + "/Source/index.js"
    },
    externals: {
        "react": "React",
        "react-dom": "ReactDOM"
    },
    output: {
        path: __dirname + "/wwwroot/build/",
        filename: "[name].js"
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                use: {
                    loader: "babel-loader",
                    options: {
                        presets: ["@babel/preset-react", "@babel/preset-env"]
                    }
                },
                exclude: /node_modules/
            }
        ]
    },
    mode: (process.env.NODE_ENV || "development").trim(),
    plugins: [
        new CleanWebpackPlugin()
    ]
};

module.exports = [applications];