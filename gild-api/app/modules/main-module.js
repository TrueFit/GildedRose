angular.module('main', ['ui.router', 'ngAnimate', 'toastr']);

// https://github.com/angular-ui/ui-router/issues/2889
angular.module('main')
    .config(['$qProvider', function ($qProvider) {
        $qProvider.errorOnUnhandledRejections(false);
    }]);
