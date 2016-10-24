(function (ng, app)
{
    "use strict";
    app.controller("searchPanelController", ["$scope", "$http", function ($scope, $http)
    {
        //$scope.activeTabIndex = 1;
        $scope.canSearch = false;

        $scope.onSelect = function ($item, $model, $label)
        {
            $scope.selectedItem = $item;
            $scope.label = $label;
        };

        $scope.findCities = function(query)
        {
            return $http.get("api/cities",
            {
                params: { query: query }
            }).then(function (response)
            {
                return response.data;
            });
        }
    }]);
})(angular, angular.module("distanceBtwCities"));