(function () {
    angular.module('processApp')
        .service('homeService', ['$q', '$http', homeService]);

    function homeService($q, $http) {
        var self = this;

        self.getBar = function () {
            return "bar";
        }
    }
}());