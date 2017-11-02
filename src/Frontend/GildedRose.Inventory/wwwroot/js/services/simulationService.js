angular
.module('app')
.service("simulationService", ['$http', '$q', function ($http, $q) {

    this.getDate = function () {
        return $q(function (success, error) {
            $http.get('/api/Simulation/GetDate')
                .then(function (response) { success(response.data); }, error);
        });
    };

    this.advanceDateByOneDay = function() {
        return $q(function (success, error) {
            $http.post('/api/Simulation/AdvanceDateByOneDay')
                .then(function (response) { success(response.data); }, error);
        });
    };

    this.setDate = function (date) {
        return $q(function (success, error) {
            $http.post('/api/Simulation/SetDate', { data: { date: date } })
                .then(function (response) { success(response.data); }, error);
        });
    };
}]);

