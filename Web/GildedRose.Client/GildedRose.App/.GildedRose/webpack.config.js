var webpack = require('webpack');
var path = require('path');


// variables
var currentDirectory = __dirname;
var isProduction = process.argv.indexOf('-p') >= 0 || process.env.NODE_ENV === 'production';
var sourcePath = path.join(currentDirectory, '../src');
var outPath = path.join(currentDirectory, '../build');


// plugins
var HtmlWebpackPlugin = require('html-webpack-plugin');
var MiniCssExtractPlugin = require('mini-css-extract-plugin');
var WebpackCleanupPlugin = require('webpack-cleanup-plugin');

module.exports = {
  context: sourcePath,
  entry: {
    app: './main.tsx'
  },
  output: {
    path: outPath,
    filename: 'bundle.js',
    chunkFilename: '[chunkhash].js'
  },
  target: 'web',
  resolve: {
    extensions: ['.js', '.ts', '.tsx'],
    // Fix webpack's default behavior to not load packages with jsnext:main module
    // (jsnext:main directs not usually distributable es6 format, but es6 sources)
    mainFields: ['module', 'browser', 'main'],
    alias: {
      app: path.resolve(currentDirectory, '../src/app/'),
      core: path.resolve(currentDirectory, "../../GildedRose.Core/src/"),
      views: path.resolve(currentDirectory, '../src/app/views/'),
      models: path.resolve(currentDirectory, '../src/app/models/'),
      components: path.resolve(currentDirectory, '../src/app/components/'),
      styles: path.resolve(currentDirectory, '../src/assets/styles/'),
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
          failOnHint: false,
          configFile: 'tslint.json'
        }
      },
      {
        test: /\.tsx?$/,
        use: [
          !isProduction && {
            loader: 'babel-loader',
            options: { plugins: ['react-hot-loader/babel'] }
          },
          'ts-loader'
        ].filter(Boolean)
      },
      {
        test: /\.(png|gif|jpg|woff|eot|ttf|svg|woff2)$/i,
        use: "file-loader?name=content/[name][hash].[ext]",
      },
      {
        test: /\.(config)$/i,
        use: "file-loader?name=[name].[ext]",
      },
      {
        test: /\.css$/,
        use: ["style-loader", "css-loader", "postcss-loader"]
      }
    ]
  },
  optimization: {
    splitChunks: {
      name: true,
      cacheGroups: {
        commons: {
          chunks: 'initial',
          minChunks: 2
        },
        vendors: {
          test: /[\\/]node_modules[\\/]/,
          chunks: 'all',
          priority: -10
        }
      }
    },
    runtimeChunk: true
  },
  plugins: [
    new webpack.EnvironmentPlugin({
      NODE_ENV: 'development', // use 'development' unless process.env.NODE_ENV is defined
      DEBUG: false
    }),
    new WebpackCleanupPlugin(),
    new MiniCssExtractPlugin({
      filename: '[contenthash].css',
      disable: !isProduction
    }),
    new HtmlWebpackPlugin({
      template: 'assets/index.html'
    })
  ],
  devServer: {
    contentBase: sourcePath,
    hot: true,
    inline: true,
    // historyApiFallback: true,
    historyApiFallback: {
      index: 'index.html'
    },
    stats: 'minimal',
    clientLogLevel: 'warning'
  },
  // https://webpack.js.org/configuration/devtool/
  devtool: isProduction ? 'hidden-source-map' : 'cheap-module-eval-source-map',
  node: {
    // workaround for webpack-dev-server issue
    // https://github.com/webpack/webpack-dev-server/issues/60#issuecomment-103411179
    fs: 'empty',
    net: 'empty'
  }
};
