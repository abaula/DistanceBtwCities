(function (ng, app)
{
    "use strict";
    app.controller("searchPanelController", ["$scope", "$http", "searchService", "overlayService", "appConstants", function ($scope, $http, searchService, overlayService, appConstants)
    {
        $scope.canSearch = false;
        $scope.formData = {};

        var createFilter = function (cityId, cityName)
        {
            return {
                cityId: cityId,
                cityName: cityName
            };
        }

        $scope.onSelect = function($item, $model, $label)
        {
            $scope.selectedItem = $item;
            $scope.label = $label;
        };

        $scope.findCities = function(query)
        {
            return $http.get(appConstants.url.getCitiesUrl,
            {
                params: { query: query }
            }).then(function (response)
            {
                return response.data;
            });
        }

        $scope.findByCityId = function()
        {
            if (ng.isUndefined($scope.selectedItem))
                return;

            overlayService.showOverlay();
            searchService.setFilter(createFilter($scope.selectedItem.id, null));
            searchService.loadPage(0);
        }

        $scope.findByCityName = function()
        {
            if (ng.isUndefined($scope.formData.cityNameQuery))
                return;

            overlayService.showOverlay();
            searchService.setFilter(createFilter(null, $scope.formData.cityNameQuery));
            searchService.loadPage(0);
        }
    }]);
})(angular, angular.module("distanceBtwCities"));