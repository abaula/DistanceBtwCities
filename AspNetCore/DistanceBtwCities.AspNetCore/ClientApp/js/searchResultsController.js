(function (ng, app)
{
    "use strict";
    app.controller("searchResultsController", ["$scope", "searchService", "paginationService", "appConstants", "overlayService", "distanceEditService",
    function ($scope, searchService, paginationService, appConstants, overlayService, distanceEditService)
    {
        var setDefaults = function()
        {
            $scope.hasResultData = false;
            $scope.routes = null;
            $scope.allRoutesCount = 0;
            $scope.paginationData = null;
        }

        $scope.edit = function(routeItem)
        {
            distanceEditService.showEditor(routeItem);
        }

        $scope.loadPage = function(pageNumber)
        {
            overlayService.showOverlay();
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
            overlayService.hideOverlay();
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

        distanceEditService.subscribeOnDistanceUpdated(function(routeId, distance)
        {
            for (var i = 0; i < $scope.routes.length; i++)
            {
                var route = $scope.routes[i];

                if(route.id !== routeId)
                    continue;

                route.distance = distance;
                break;
            }            
        });

        setDefaults();        
    }]);
})(angular, angular.module("distanceBtwCities"));