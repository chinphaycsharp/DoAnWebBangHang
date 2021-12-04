(function (app) {
    app.controller('loginController', ['$scope', 'loginService', '$injector', 'notificationService', '$rootScope','localStorageService',
        function ($scope, loginService, $injector, notificationService, $rootScope, localStorageService) {

            $scope.loginData = {
                userName: "",
                password: ""
            };

            $scope.loginSubmit = function () {
                loginService.login($scope.loginData.userName, $scope.loginData.password).then(function (response) {
                    if (response != null && response.error != undefined) {
                        notificationService.displayError(response.error);
                    }
                    else {
                        //$rootScope.userName = $scope.loginData.userName;
                        //$rootScope.password = $scope.loginData.password;
                        localStorageService.set("userName", $scope.loginData.userName);
                        localStorageService.set("passWord", $scope.loginData.password);
                        var stateService = $injector.get('$state');
                        stateService.go('home');
                    }
                });
            }
        }]);
})(angular.module('tedushop'));