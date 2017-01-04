(function (ng, app)
{
    "use strict";
    app.controller("searchResultsController", ["$scope", "searchService", "paginationService", "appConstants", function ($scope, searchService, paginationService, appConstants)
    {
        var setDefaults = function()
        {
            $scope.hasResultData = false;
            $scope.routes = null;
            $scope.allRoutesCount = 0;
            $scope.paginationData = null;
        }

        $scope.loadPage = function(pageNumber)
        {
            searchService.loadPage(pageNumber);
        }

        var isDataValid = function(data)
        {
            if (ng.isUndefined(data) || ng.isUndefined(data.routes) || ng.isUndefined(data.allFoundRoutesCount)
                || !ng.isArray(data.routes))
                return false;

            return true;
        }

        var setPagesInfo = function(query)
        {
            var currentPageNum = Math.floor(query.offset / appConstants.rowCount);
            var allPagesNum = Math.floor(($scope.allRoutesCount - 1) / appConstants.rowCount);
            $scope.paginationData = paginationService.buildPaginationData(currentPageNum, allPagesNum);
        }

        searchService.subscribeOnPageLoaded(function (data)
        {
            setDefaults();
            
            if (!isDataValid(data))
                return;

            if (data.routes.length < 1)
                return;

            $scope.hasResultData = true;
            $scope.routes = data.routes;
            $scope.allRoutesCount = data.allFoundRoutesCount;
            setPagesInfo(searchService.getQuery());
        });

        setDefaults();        
    }]);
})(angular, angular.module("distanceBtwCities"));