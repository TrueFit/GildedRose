angular.module("main")
    .service("dayService", function ($http) {
        var getDay = function () { return $http.post("api/get-day"); };
        var advanceDay = function () { return $http.post("api/advance-day"); };

        return {
            getDay: getDay,
            advanceDay: advanceDay
        };
    });