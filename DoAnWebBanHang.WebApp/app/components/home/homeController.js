(function (app) {
    app.controller('homeController', homeController);
    homeController.$inject = ['$rootScope', 'apiService'];
    function homeController($rootScope, apiService) {

        var username = $rootScope.userName;
        var password = $rootScope.password;
        console.log(username);
        console.log(password);
        function getCurrentUser() {
            apiService.get('/api/home/getcurrentuser?username=' + username + '&password=' + password,
                null,
                function (response) {
                    console.log(response.data);
                    /*$rootscope.currentUser = response.data;*/
                }, function (response) {
                });

        }
        getCurrentUser();
    }
})(angular.module('tedushop'));