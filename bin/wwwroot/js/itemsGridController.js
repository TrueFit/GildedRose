angular
.module('app')
.controller('itemsGridController', ['$scope', '$attrs', 'inventoryService', function ($scope, $attrs, inventoryService) {

    $scope.gridMode = 'available';
    $scope.setGridMode = function (mode) { $scope.gridMode = mode; refreshItemsGridColumns(); }

    $scope.gridApi = null;
    $scope.gridPagination = {
        page: 1,
        pageSize: 10,
        totalItems: 0,
        sortOrder: null
    };

    var baseColumns = [
        {
            displayName: 'Name',
            name: 'name',
            field: 'name',
            width: '30%',
            cellTooltip: function (row) { return row.entity.description; }
        },
        {
            displayName: 'Type',
            name: 'itemType',
            field: 'itemType.name',
            width: '25%',
        },
        {
            displayName: 'Quality',
            name: 'quality',
            field: 'currentQuality',
            width: '15%',
        },
        {
            displayName: 'Sell By Date',
            name: 'sellByDate',
            field: 'sellByDate',
            width: '20%',
            type: 'date',
            cellFilter: 'date:"MM/dd/yyyy"'
        }
    ];

    var availableColumns = [
        {
            displayName: '',
            name: 'sellLink',
            enableSorting: false,
            width: '10%',
            cellTemplate: '<div class="ui-grid-cell-contents" title="TOOLTIP"><button type="button" ng-click="grid.appScope.sellItem(row.entity.inventoryItemId)">Sell</button></div>'
        }
    ];

    var expiredCoulumns = [
        {
            displayName: '',
            name: 'discardLink',
            enableSorting: false,
            width: '10%',
            cellTemplate: '<div class="ui-grid-cell-contents" title="TOOLTIP"><button type="button" ng-click="grid.appScope.discardItem(row.entity.inventoryItemId)">X</button></div>'
        }
    ];

    $scope.itemsGrid = {
        columnDefs: baseColumns,
        paginationPageSizes: [10],
        paginationPageSize: $scope.gridPagination.pageSize,
        useExternalPagination: true,
        useExternalSorting: true,
        onRegisterApi: function (gridApi) {
            gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                if (sortColumns.length === 0) {
                    $scope.gridPagination.sortOrder = null;
                } else {
                    var columns = ['test', 'test desc'];
                    columns = sortColumns.map(function (sc) { return sc.name + (sc.sort.direction === 'desc' ? ' desc' : ''); });
                    $scope.gridPagination.sortOrder = columns;
                }
                $scope.fetchGridData();
            });
            gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                $scope.gridPagination.page = newPage;
                $scope.gridPagination.pageSize = pageSize;
                $scope.fetchGridData();
            });
        }
    };

    var refreshItemsGridColumns = function () {
        $scope.itemsGrid.columnDefs =
            $scope.gridMode === 'available'
                ? baseColumns.concat(availableColumns)
                : baseColumns.concat(expiredCoulumns);
    }
    refreshItemsGridColumns();

    var getAvailableItems = function (sortOrder, page, pageSize) {
        return inventoryService.getAvailableItems(sortOrder, page, pageSize);
    };

    var getExpiredItems = function (sortOrder, page, pageSize) {
        return inventoryService.getExpiredItems(sortOrder, page, pageSize);
    };
    
    $scope.fetchGridData = function () {
        var request = null;

        if ($scope.gridMode === 'expired')
            request = getExpiredItems($scope.gridPagination.sortOrder, $scope.gridPagination.page, $scope.gridPagination.pageSize);
        else if ($scope.gridMode === 'available')
            request = getAvailableItems($scope.gridPagination.sortOrder, $scope.gridPagination.page, $scope.gridPagination.pageSize);

        if (request) {
            request.then(function (data) {
                $scope.gridPagination.totalItems = data.totalItems;
                $scope.itemsGrid.data = data.items;
                $scope.itemsGrid.totalItems = data.totalItems;
            });
        }
    };

    $scope.sellItem = function (itemId) {
        inventoryService.sellItem(itemId)
            .then(function () { $scope.fetchGridData(); })
    };

    $scope.discardItem = function (itemId) {
        inventoryService.discardItem(itemId)
            .then(function () { $scope.fetchGridData(); })
    };


    $scope.$on('reloadItemsGrid', function (e) {
        $scope.gridPagination.page = 1;
        $scope.fetchGridData();
    });
}])
.directive('grExpiredGrid', [function () {
    return { link: function (scope) { scope.setGridMode('expired'); } };
}]);