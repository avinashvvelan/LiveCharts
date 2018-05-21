(function () {

    angular.module('processApp')
        .factory('mainService', ['$http', '$q', mainService]);

    function mainService($http, $q) {
        var self = this;

        self.getFoo = function () {
            return "foo";
        }

        return {
            getFoo: self.getFoo
        };
    }
}());