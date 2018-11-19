angular.module("main")
    .controller("homeController", function ($scope, $http) {
        $scope.model = {};

        $http.get("app/pages/home/write-up.txt")
            .then(function (response) {
                $scope.model.writeUp = response.data;
            }, function (err) { });
});