(function (ng, app)
{
    "use strict";
    app.service("distanceEditService", ["$http", "appConstants", function ($http, appConstants)
    {
        var showEditorHandlers = [];
        var updateDistanceHandlers = [];

        var subscribeOnShowEditor = function (handler)
        {
            showEditorHandlers.push(handler);
        }

        var subscribeOnDistanceUpdated = function (handler)
        {
            updateDistanceHandlers.push(handler);
        }

        var showEditor = function (routeItem)
        {
            ng.forEach(showEditorHandlers, function (handler)
            {
                handler(routeItem);
            });
        }

        var onDistanceUpdated = function(routeId, distance)
        {
            ng.forEach(updateDistanceHandlers, function (handler)
            {
                handler(routeId, distance);
            });
        }

        var updateDistance = function(routeId, distance)
        {
            var data = { id: routeId, distance: distance };

            $http.put(appConstants.url.updateRouteUrl, data)
            .then(
                function (successResponse)
                {
                    onDistanceUpdated(routeId, distance);
                },
                function (failureResponse)
                {
                });
        }

        return {
            subscribeOnShowEditor: subscribeOnShowEditor,
            subscribeOnDistanceUpdated: subscribeOnDistanceUpdated,
            showEditor: showEditor,
            updateDistance: updateDistance
        };
    }]);
})(angular, angular.module("distanceBtwCities"));