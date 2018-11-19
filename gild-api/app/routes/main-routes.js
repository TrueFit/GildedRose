
angular.module('main')
    .config(function ($stateProvider, $urlRouterProvider) {

        var toCamel = function (text) {
            return text.replace(/-([a-z])/g, function (g) { return g[1].toUpperCase(); });
        };

        var milliseconds = new Date().getMilliseconds();

        var preventCache = function (url) {
            return url + "?t=" + milliseconds;
        };

        $urlRouterProvider.otherwise('/home');

        var getTemplateUrl = function (name) {
            return preventCache('app/pages/' + name + '/' + name + '-template.html');
        };

        var getListTemplateUrl = function (name) {
            return preventCache('app/pages/' + name + '/' + 'list' + '/' + name + '-list-template.html');
        };

        var getDetailTemplateUrl = function (name) {
            return preventCache('app/pages/' + name + '/' + 'detail' + '/' + name + '-detail-template.html');
        };

        var menuView = { templateUrl: getTemplateUrl('menu'), controller: 'menuController' };
        var dayView = { templateUrl: getTemplateUrl('day'), controller: 'dayController' };

        var getView = function (name) {
            var view = {};
            view.main = { templateUrl: getTemplateUrl(name), controller: toCamel(name) + 'Controller' };
            view.menu = menuView;
            view.day = dayView;

            return view;
        };

        var getListView = function (name) {
            var view = {};
            view.main = { templateUrl: getListTemplateUrl(name), controller: toCamel(name) + 'ListController' };
            view.menu = menuView;
            view.day = dayView;

            return view;
        };

        var getDetailView = function (name) {
            var view = {};
            view.main = { templateUrl: getDetailTemplateUrl(name), controller: toCamel(name) + 'DetailController' };
            view.menu = menuView;
            view.day = dayView;

            return view;
        };

        var addPage = function (name) {
            $stateProvider.state(name, { url: '/' + name, views: getView(name) });
        };

        var addListPage = function (name) {
            $stateProvider.state(name + '-list', { url: '/' + name + '-list', views: getListView(name) });
        };

        var addDetailPage = function (name) {
            $stateProvider.state(name + '-detail', { url: '/' + name + '-detail/:id', views: getDetailView(name) });
        };

        addPage('home');
        addListPage("inventory");
        addDetailPage("inventory");
        addPage('operations');
        addPage('trash');
    });
