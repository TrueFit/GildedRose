angular
.module('app')
.controller('simulationController', ['$scope', 'simulationService', function($scope, simulationService) {

    $scope.isLoaded = false;
    $scope.currentDate = null;

    var updateCurrentDate = function (date) {
        $scope.currentDate = date;
        $scope.$broadcast('reloadItemsGrid');
    };

    $scope.setDate = function (date) {
        simulationService.setDate()
            .then(function (data) {
                updateCurrentDate(data.currentDate);
            });
    };

    $scope.advanceDateByOneDay = function () {
        simulationService.advanceDateByOneDay()
            .then(function (data) {
                updateCurrentDate(data.currentDate);
            });
    };         

    $scope.initController = function () {
        simulationService.getDate().then(function (date) {
            updateCurrentDate(date);
            $scope.isLoaded = true;
        });
    };
}])
.directive('grSimulationTime', ['$interval', function($interval) {

    this.link = function (scope, element) {

        function tick() {
            if (scope.currentDate) {
                var d = new Date(scope.currentDate);
                d.setSeconds(d.getSeconds() + 1);
                scope.currentDate = d;
            }
        };

        var intervalId = $interval(tick, 1000);

        element.on('$destroy', function () {
            $interval.cancel(intervalId);
        });
    };

    return { link: link };
}]);