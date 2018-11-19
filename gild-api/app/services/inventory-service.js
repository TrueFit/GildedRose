angular.module("main")
    .service("inventoryService", function ($http) {
        var getInventory = function () { return $http.post("api/get-inventory"); };

        return {
            getInventory: getInventory
        };
    });