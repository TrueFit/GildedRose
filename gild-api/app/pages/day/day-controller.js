angular.module("main")
    .controller("dayController", function ($scope, $rootScope, dayService, apocalypseService, alertService, errorService) {
        $scope.model = {};

        $scope.onNextDayClicked = function () {
            var onSuccess = function (response) {
                $rootScope.$broadcast("dayUpdated", { message: "Current Day = " + response.data });
                alertService.info("Advanced to the next day!");
                $scope.model.currentDay = response.data;
            };

            dayService.advanceDay().then(onSuccess, errorService.onError);            
        };

        $scope.onResetTheUniverseClicked = function () {
            var didUserConfirm = confirm("TODO: Use a non-blocking dialog.\r\n\r\n" + "Seems kind of drastic.\r\nAre you sure about this?");
            if (didUserConfirm) {
                resetTheUniverse();
            }
        };

        var resetTheUniverse = function () {
            var onSuccess = function (response) {
                $scope.model.currentDay = response.data;
                $rootScope.$broadcast("dayUpdated", { message: "Current Day = " + response.data });
                loadDay();

                alertService.success("Make it so!");
            };

            apocalypseService.resetTheUniverse().then(onSuccess, errorService.onError);
        };

        var loadDay = function () {
            var onSuccess = function (response) {
                $scope.model.currentDay = response.data;                
            };

            dayService.getDay().then(onSuccess, errorService.onError);
        };

        loadDay();
});