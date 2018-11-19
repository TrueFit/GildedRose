angular.module("main")
    .controller("inventoryListController", function ($scope, alertService, iconService, inventoryService) {
        $scope.model = {
            inventory: { isLoading: false, data: null }
        };

        var loadData = function () {
            var onFailure = function (err) {
                $scope.model.inventory.isLoading = false;
                alertService.onError(err);
            };

            var onSuccess = function (response) {
                try {
                    if (response.data) {
                        for (var i = 0; i < response.data.length; i++) {
                            var inventoryItem = response.data[i];
                            inventoryItem.icon = iconService.getIcon(inventoryItem);
                        }
                    }

                    $scope.model.inventory.data = response.data;
                }
                finally {
                    $scope.model.inventory.isLoading = false;
                }
            };

            $scope.model.inventory.isLoading = true;
            inventoryService.getInventory().then(onSuccess, onFailure);
        };

        $scope.$on("dayUpdated", function () {
            loadData();
        });

        loadData();
});