angular.module("main")
    .controller("trashController", function ($scope, $http) {
        $scope.model = {
            trash: { isLoading: false, data: null }
        };

        var loadData = function () {
            var onFailure = function (err) {
                alert("Error: " + JSON.stringify(err));
            };

            $http.post("api/get-trash")
                .then(function (response) {
                    $scope.model.trash.data = response.data;
                }, onFailure);
        };

        $scope.$on("dayUpdated", function () {
            loadData();
        });

        loadData();
});