

var myApp = angular.module('myModule', []);

myApp.controller('myController', myController);
myController.$inject = ['$scope'];
//
function myController($scope) {
    $scope.message = "Hello World";
}