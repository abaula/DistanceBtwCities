(function (ng, app)
{
    "use strict";
    app.service("searchService", ["$http", "appConstants", function ($http, appConstants)
    {
        var pageLoadedHandlers = [];
        var filter = null;
        var query = null;

        var subscribeOnPageLoaded = function (handler)
        {
            pageLoadedHandlers.push(handler);
        }

        var setFilter = function(value)
        {
            filter = value;
        }

        var getFilter = function()
        {
            return filter;
        };

        var getQuery = function ()
        {
            return query;
        };

        var loadPage = function (pageNum)
        {
            createQuery(pageNum);

            $http.get("api/routes",
            {
                params: getQuery()
            }).then(function (response)
            {
                ng.forEach(pageLoadedHandlers, function (handler)
                {
                    handler(response.data);
                });
            });
        }

        var createQuery = function (pageNum)
        {
            query = {
                cityId: filter.cityId,
                query: filter.cityName,
                maxDistance: appConstants.maxDistanse,
                offset: pageNum * appConstants.rowCount,
                rows: appConstants.rowCount
            };
        }

        return {
            subscribeOnPageLoaded: subscribeOnPageLoaded,
            setFilter: setFilter,
            getFilter: getFilter,
            getQuery: getQuery,
            loadPage: loadPage
        };
    }]);
})(angular, angular.module("distanceBtwCities"));