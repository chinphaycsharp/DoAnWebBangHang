/// <reference path="../../../assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('tedushop.tag_categories', ['tedushop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('tag_categories', {
            url: "/tag_categories",
            parent: 'base',
            templateUrl: "/app/components/tag_categories/tagCategoriesListView.html",
            controller: "tagCategoriesListController"
        })
            .state('add_tag_category', {
            url: "/add_tag_category",
            parent: 'base',
            templateUrl: "/app/components/tag_categories/tagCategoriesAddView.html",
            controller: "tagCategoryAddController"
            })
            //.state('edit_product_category', {
        //    url: "/edit_tag_category/:id",
        //    parent: 'base',
        //    templateUrl: "/app/components/tag_categories/tagCategoriesEditView.html",
        //    controller: "tagCategoryEditController"
        //});;
    }
})();