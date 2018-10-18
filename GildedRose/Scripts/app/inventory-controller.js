angular.module('GildedRoseApp', [])
    .controller('InventoryCtrl', function ($scope, $http) {
        $scope.items = [];
        $scope.working = false;
        $scope.search = {};

        $scope.getInventory = function () {
            $scope.working = true;
            $scope.options = [];

            $http.get("/api/Inventory").then(function successCallback(response) {
                $scope.items = response.data;
                $scope.working = false;
            }, function errorCallback(response) {
                $scope.working = false;
            });
        };

        $scope.searchByName = function () {
            $scope.working = true;
            $scope.options = [];

            $http.get("/api/Inventory/Get?name=" + $scope.search.name).then(function successCallback(response) {
                $scope.items = new Array();
                $scope.items[0] = response.data;
                $scope.working = false;
            }, function errorCallback(response) {
                $scope.working = false;
            });
        };

        $scope.getTrash = function () {
            $scope.working = true;
            $scope.options = [];

            $http.get("/api/Inventory/Trash").then(function successCallback(response) {
                $scope.items = response.data;
                $scope.working = false;
            }, function errorCallback(response) {
                $scope.working = false;
            });
        };

        $scope.endDay = function () {
            $scope.working = true;
            $scope.options = [];

            $http.post("/api/Inventory/EndDay").then(function successCallback(response) {
                $scope.items = response.data;
                $scope.working = false;
            }, function errorCallback(response) {
                $scope.working = false;
            });
        };

    });