(function (app)
{
    "use strict";
    app.constant("appConstants", {
        maxDistanse: 100000,
        rowCount: 10,
        paginationSize: 10,
        url:
        {
            getCitiesUrl: "api/cities",
            getRoutesUrl: "api/routes",
            updateRouteUrl: "api/routes"
        }        
    });
})(angular.module("distanceBtwCities"));