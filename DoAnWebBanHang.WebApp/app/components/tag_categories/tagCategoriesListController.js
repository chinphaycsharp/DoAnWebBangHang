(function (app) {
    app.controller('tagCategoriesListController', tagCategoriesListController);

    tagCategoriesListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function tagCategoriesListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.tagCategories = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getTagCagories = getTagCagories;
        $scope.keyword = '';
        $scope.search = search;
        $scope.name = '';
        $scope.id = 0;

        $scope.deleteProductCategory = deleteProductCategory;

        $scope.selectAll = selectAll;

        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedTagCategories: JSON.stringify(listId)
                }
            }
            apiService.del('/api/postcategory/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.tagCategories, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.tagCategories, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("tagCategories", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteProductCategory(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/postcategory/remove', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

        function search() {
            getTagCagories();
        }

        function getTagCagories(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 2
                }
            }
            apiService.get('/api/postcategory/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                $scope.tagCategories = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load productcategory failed.');
            });
        }

        $scope.getTagCagories();

        $scope.getName = function (id, name) {
            $scope.id = id;
            $scope.name = name;
            console.log(id + ' - ' + name);
        }

        $scope.delete = function (id) {
            console.log(id);
        }
    }
})(angular.module('tedushop.tag_categories'));