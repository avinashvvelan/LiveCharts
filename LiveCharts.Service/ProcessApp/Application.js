(function () {
    var processApp = angular.module('processApp', ['ui.router']);

    processApp.config(
        function ($stateProvider, $urlRouterProvider) {


            $urlRouterProvider.otherwise('/');

            $stateProvider
                .state('home', {
                    url: '/',
                    templateUrl: '/ProcessApp/templates/home.html',
                    controller: 'homeController as hc',
                    data: { displayHome: 'Home' }
                });
        });
}());


