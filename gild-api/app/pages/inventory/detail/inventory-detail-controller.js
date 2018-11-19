angular.module("main")
    .controller("inventoryDetailController", function ($scope, $http, iconService) {
        $scope.model = {};

        var onFailure = function (err) {
            alert("Error: " + JSON.stringify(err));
        };

        var onSuccess = function (response) {
            var inventoryItem = response.data;
            inventoryItem.icon = iconService.getIcon(inventoryItem);
            $scope.model.inventoryItem = inventoryItem
        };

        $http.post("api/get-inventory-item")
            .then(onSuccess, onFailure);
});