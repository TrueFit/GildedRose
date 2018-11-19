angular.module("main")
    .service("apocalypseService", function ($http) {
        var resetTheUniverse = function () { return $http.post("api/reset-the-universe"); };

        return {
            resetTheUniverse: resetTheUniverse
        };
    });