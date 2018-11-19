angular.module('main')
    .directive('loading', function () {
        var directivesRoot = 'app/directives/';

        return {
            restrict: 'E',
            templateUrl: directivesRoot + 'loading-directive/loading-template.html',
        };
    });


