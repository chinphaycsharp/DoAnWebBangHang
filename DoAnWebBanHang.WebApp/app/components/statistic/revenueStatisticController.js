(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function revenueStatisticController($scope, apiService, notificationService, $filter) {
        $scope.tabledata = [];
    /*        $scope.labels = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];*/
        $scope.labels = [];
        $scope.series = ['Doanh số', 'Lợi nhuận'];

        $scope.chartdata = [
        ];
        //$scope.chartdata = [
        //    [65, 56, 80, 81, 56, 55, 40],
        //    [28, 48, 40, 19, 86, 27, 90]
        //];
        function getStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: 12,
                    toDate: 1
                }
            }
            apiService.get('/api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.tabledata = response.data;
                var labels = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
                $.each(response.data, function (i, item) {
                    labels.push(item.month);
                    revenues.push(item.Revenues);
                    benefits.push(item.Benefit);
                });
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
                $scope.labels = labels;
            }, function (response) {
                notificationService.displayError('Không thể tải dữ liệu');
            });
        }

        getStatistic();
    }

})(angular.module('tedushop.statistics'));