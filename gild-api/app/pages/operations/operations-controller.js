angular.module("main")
    .controller("operationsController", function ($scope, $http, toastr, errorService) {
        $scope.model = {};

        $scope.onNextDayClicked = function () {
            toastr.warning("You can hear the monsters howling outside, but you are safe inside the inn.");
            advanceDay();
        };

        $scope.onResetTheUniverseClicked = function () {
            var didUserConfirm = confirm("TODO: Use a non-blocking dialog.\r\n\r\n" + "Seems kind of drastic.\r\nAre you sure about this?");
            if (didUserConfirm) {
                resetTheUniverse();
            }
        };

        $scope.onToastClicked = function () {
            toastr.success('I just tell them that I like toast.', 'Toastr fun!');
        };

        $scope.onErrorClicked = function () {
            errorService.onError({ message: "mistakes were made." });
        };

        var resetTheUniverse = function () {
            var onSuccess = function (response) {
                $scope.model.currentDay = response.data;
                getDay();
                alert("Make it so!");
            };

            onError = function (err) { alert("Error: " + err); };

            $http.post("api/reset-the-universe").then(onSuccess, onError);
        };

        var advanceDay = function () {
            var onSuccess = function (response) {
                $scope.model.currentDay = response.data;
                toastr.success("Advanced!");
            };

            onError = function (err) { alert("Error: " + err); };

            $http.post("api/advance-day").then(onSuccess, onError);
        };

        var getDay = function () {
            var onSuccess = function (response) {
                $scope.model.currentDay = response.data;
            };

            onError = function (err) { alert("Error: " + JSON.stringify(err)); };

            $http.post("api/get-day").then(onSuccess, onError);
        };

        getDay();
});