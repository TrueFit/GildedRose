angular
.module('app')
.service("inventoryService", ['$http', '$q', function ($http, $q) {
    this.getItemTypes = function () {
        return $q(function (success, error) {
            $http.get('/api/Inventory/GetItemTypes').then(
                function (response) { success(response.data); },
                function (response) { error(response); });
        });
    };

    this.getQualityDeltaStrategies = function () {
        return $q(function (success, error) {
            $http.get('/api/Inventory/GetQualityDeltaStrategies')
                .then(function (response) { success(response.data); }, error);
        });
    };

    this.addNewItem = function (itemTypeId, name, description, quality, sellByDate) {
        return $q(function (success, error) {
            $http.post(
                '/api/Inventory/AddNewItem', 
                {
                    //data: JSON.stringify({
                        ItemTypeId: itemTypeId,
                        Name: name,
                        Description: description,
                        Quality: quality,
                        SellByDate: sellByDate
                    //})
                })
                .then(function (response) { success(response.data); }, error);
        });
    };

    this.getAvailableItems = function (sortOrder, page, pageSize) {
        return $q(function (success, error) {
            $http.get(
                '/api/Inventory/GetAvailableItems',
                {
                    params: { sortOrder: sortOrder, page: page, pageSize: pageSize }
                }).then(function (response) { success(response.data); }, error);
        });
    };

    this.getExpiredItems = function (sortOrder, page, pageSize) {
        return $q(function (success, error) {
            $http.get(
                '/api/Inventory/GetExpiredItems',
                {
                    params: { sortOrder: sortOrder, page: page, pageSize: pageSize }
                })
                .then(function (response) { success(response.data); }, error);
        });
    };

    this.discardItem = function (itemId) {
        return $q(function (success, error) {
            $http.post('/api/Inventory/DiscardItem?itemId=' + itemId)
            .then(function (response) { success(response.data); }, error);
        });
    };

    this.sellItem = function (itemId) {
        return $q(function (success, error) {
            $http.post('/api/Inventory/SellItem?itemId=' + itemId )
                .then(function (response) { success(response.data); }, error);
        });
    };
}]);