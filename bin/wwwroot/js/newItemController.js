angular
.module('app')
.controller('newItemController', ['$scope', 'inventoryService', function($scope, inventoryService) {

    $scope.isControlVisible = false;
    $scope.isSaving = false;

    $scope.itemTypes = [];
    $scope.selectedItemType = null;
    $scope.itemName = '';
    $scope.itemDescription = '';
    $scope.itemQuality = 0;
    $scope.itemSellByDate = null;

    $scope.initController = function () {
        inventoryService.getItemTypes()
            .then(function (result) {
                if (result.wasSuccessful)
                    $scope.itemTypes = result.data;
            });
    };

    $scope.showControlClick = function () { clearControl(); $scope.isControlVisible = true; };

    var clearControl = function () {

        $scope.selectedItemType = null;
        $scope.itemName = '';
        $scope.itemDescription = '';
        $scope.itemQuality = 0;
        var d = $scope.$parent.currentDate ? new Date($scope.$parent.currentDate) : new Date();
        d.setDate(d.getDate() + 1, 0, 0, 0, 0); //Tomorrow
        $scope.itemSellByDate = d;
    };
    clearControl();

    $scope.canSave = function () {

        if ($scope.selectedItemType !== null
            && $scope.itemName.trim() !== ''
            && $scope.itemQuality >= $scope.selectedItemType.minQuality
            && $scope.itemQuality <= $scope.selectedItemType.maxQuality)
            return true;
        return false;
    };

    $scope.saveClick = function () {

        $scope.isSaving = true;
        inventoryService.addNewItem(
            $scope.selectedItemType.inventoryItemTypeId,
            $scope.itemName,
            $scope.itemDescription,
            $scope.itemQuality,
            $scope.selectedItemType.isSellByDateRequired ? $scope.itemSellByDate : null)
            .then(function (data) {
                if (data.wasSuccessful) {
                    $scope.isControlVisible = false;
                    clearControl();
                    $scope.$emit('reloadItemsGrid');
                }
                else throw data.errors;
            })
            .finally(function () { $scope.isSaving = false; });
    };

    $scope.cancelClick = function () {
        $scope.isControlVisible = false;
        clearControl();
    };

    $scope.$watch('selectedItemType', function (newValue, oldValue, scope) {
        if (newValue !== null) {
            //Clamp item quality to min max range of the selected item type.
            scope.itemQuality = Math.max(Math.min(scope.itemQuality, newValue.maxQuality), newValue.minQuality);
        }
    });
}]);