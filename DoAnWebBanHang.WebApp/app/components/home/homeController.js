(function (app) {
    app.controller('homeController', homeController);
    homeController.$inject = ['$rootScope', 'apiService', 'localStorageService'];
    function homeController($rootScope, apiService, localStorageService) {

        var username = localStorageService.get("userName");
        var password = localStorageService.get("passWord");
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