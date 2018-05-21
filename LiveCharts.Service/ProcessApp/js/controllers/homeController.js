(function (module) {
    module.controller('homeController', ['$scope', 'mainService', 'homeService', homeController]);

    function homeController($scope, mainService, homeService) {

        $scope.testData = 'Hi There. This is from home controller';

        var self = this;
        self.sampleVM = new sampleViewModel(100, 'Example 1', 'One');
        self.foo = mainService.getFoo();
        self.bar = homeService.getBar();
        //self.stockVM = [];

        self.stockVM = {};

        //var ticker = $.connection.processHub;
        var processChangeNotificationConnection = $.connection.processHub;

        function load() {

            // Add a client-side hub method that the server will call
            /*ticker.client.updateStockPrice = function (stocks) {
                self.stockUpdate(stocks);
            }

            $.connection.hub.start().done(initializeSignalR);*/

            processChangeNotificationConnection.client.broadcastMessage = function (value) {
                console.log(value);
            }

            $.connection.hub.start().done(function () {
                console.log("connected");
            });

        }

        function initializeSignalR() {
            ticker.server.getAllStocks().done(self.stockCallBack);
        }


        self.stockCallBack = function (stocks) {

            for (var stockInd = 0; stockInd < stocks.length; stockInd++){
                var stock = stocks[stockInd];
                self.stockVM[stock.Symbol] = new stockViewModel(stock.Symbol, stock.Price, stock.DayOpen, stock.Change, stock.PercentChange);
            }
            $scope.$apply();
        }


        self.stockUpdate = function (stock) {
            //self.stockVM.push(new stockViewModel(stocks.Symbol, stocks.Price, stocks.DayOpen, stocks.Change, stocks.PercentChange));
            if (self.stockVM[stock.Symbol] === undefined) {
                self.stockVM[stock.Symbol] = new stockViewModel(stock.Symbol, stock.Price, stock.DayOpen, stock.Change, stock.PercentChange);
            }
            else {
                self.stockVM[stock.Symbol].Symbol = stock.Symbol;
                self.stockVM[stock.Symbol].Price = stock.Price;
                self.stockVM[stock.Symbol].DayOpen = stock.DayOpen;
                self.stockVM[stock.Symbol].Change = stock.Change;
                self.stockVM[stock.Symbol].PercentChange = stock.PercentChange;
            }
            $scope.$apply();
        }

        load();

    }

    function sampleViewModel(id, key, value) {
        var self = this;
        self.id = id;
        self.key = key;
        self.value = value;
    }




    function stockViewModel(symbol, price, dayOpen, change, percentChange) {
        var self = this;
        self.Symbol = symbol;
        self.Price = price;
        self.DayOpen = dayOpen;
        self.Change = change;
        self.PercentChange = (percentChange * 100).toFixed(2) + '%';
        self.Direction = change === 0 ? '' : change >= 0 ? '▲' : '▼';
    }

    function stockDetailViewModel(symbol, price, dayOpen, change, percentChange) {
        var self = this;
        self.Symbol = symbol;
        self.Price = price;
        self.DayOpen = dayOpen;
        self.Change = change;
        self.PercentChange = (percentChange * 100).toFixed(2) + '%';
        self.Direction = change === 0 ? '' : change >= 0 ? '▲' : '▼';
    }

}(angular.module("processApp")));