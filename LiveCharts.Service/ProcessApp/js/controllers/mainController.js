(function () {
    angular.module('processApp')
        .controller('mainController', ['$state', '$scope', mainController]);
    function mainController($state, $scope) {
        $scope.testData = "hello";
    }
}());