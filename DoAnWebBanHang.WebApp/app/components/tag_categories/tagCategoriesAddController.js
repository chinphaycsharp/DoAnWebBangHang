(function (app) {
    app.controller('tagCategoryAddController', tagCategoryAddController);

    tagCategoryAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function tagCategoryAddController(apiService, $scope, notificationService, $state) {
        $scope.tagCategory = {
            CreatedDate: new Date(),
            Status: true,
        }

        $scope.AddTagCategory = AddTagCategory;

        function AddTagCategory() {
            apiService.post('/api/postcategory/add', $scope.tagCategory,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                    $state.go('tag_categories');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
        function loadParentTag() {
            apiService.get('/api/postcategory/getallparents', null, function (result) {
                $scope.parentTag = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }

        loadParentTag();
    }

})(angular.module('tedushop.tag_categories'));