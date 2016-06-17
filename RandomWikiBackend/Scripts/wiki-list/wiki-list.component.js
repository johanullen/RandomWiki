'use strict';

angular.
    module('wikiList').
    component('wikiList', {
        templateUrl: 'Scripts/wiki-list/wiki-list.template.html',
        controller: function WikiListController($http, $sce) {
            var self = this;
            
            self.uri = 'https://en.wikipedia.org/wiki/Main_Page'; // Variable for view
            self.safeUri = $sce.trustAsResourceUrl(self.uri); // Safe variable of above
            self.orderProp = 'uri'; // Variable for sorting
            // Load list at page load
            $http.get('api/wikilinks/').then(function (response) {
                self.pages = response.data;
            });
            // Get 10 new random pages button
            self.randomize = function () {
                $http.post('api/wikilinks/', {}).then(function (response) {
                    self.pages = response.data;
                });
            };
            // Clear database button
            self.clear = function () {
                $http.delete('api/wikilinks/', {}).then(function (response) {
                    self.pages = response.data;
                });
            };
            // Set view to clicked wiki page
            self.setUri = function (uri) {
                self.uri = uri;
                self.safeUri = $sce.trustAsResourceUrl(self.uri);
            };
            // Update wiki page category
            self.updateCategory = function (page) {
                console.log("page:", page);
                $http.patch('api/wikilinks/?id=' + page.id + '&category=' + page.category, {}).then(function (response) {
                    self.pages = response.data;
                });
            };
        }
    }
);