angular.module("main")
    .controller("menuController", function ($scope, dayService, alertService, errorService) {
        $scope.model = {};

        $scope.model.menuItems = [
            { title: "Home", link: "home", icon: "fas fa-home" },
            { title: "Inventory", link: "inventory-list", icon: "fas fa-box" },
            { title: "Trash", link: "trash", icon: "far fa-trash-alt" },
            // { title: "Operations", link: "operations", icon: "fas fa-cogs" }            
        ];

        $scope.onNextDayClicked = function () {
            var onSuccess = function (response) {
                alertService.info("Advanced to the next day!");
                // alertService.info(response);
                $scope.model.currentDay = response.data;
            };

            dayService.advanceDay().then(onSuccess, errorService.onError);
            
        };

        var loadData = function () {
            var onSuccess = function (response) {
                // alertService.success("Advanced to the next day.");
                $scope.model.currentDay = response.data;
            };

            var onError = function (err)
            {
                alertService.error(err);
            };

            dayService.getDay().then(onSuccess, onError);
        };

        loadData();
});