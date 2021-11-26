(function (app) {
    app.controller('loginController', ['$scope', 'loginService', '$injector', 'notificationService','$rootScope',
        function ($scope, loginService, $injector, notificationService, $rootScope) {

            $scope.loginData = {
                userName: "",
                password: ""
            };

            $scope.loginSubmit = function () {
                loginService.login($scope.loginData.userName, $scope.loginData.password).then(function (response) {
                    if (response != null && response.error != undefined) {
                        notificationService.displayError("Đăng nhập không đúng.");
                    }
                    else {
                        $rootScope.userName = $scope.loginData.userName;
                        $rootScope.password = $scope.loginData.password;
                        var stateService = $injector.get('$state');
                        stateService.go('home');
                    }
                });
            }
        }]);
})(angular.module('tedushop'));