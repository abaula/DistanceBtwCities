(function (app)
{
    "use strict";
    app.constant("appConstants", {
        maxDistanse: 100000,
        rowCount: 10,
        paginationSize: 10,
        dataRowIdPrefix: "data-row-id-",
        distanceTdClassName: "route-distance",
        buttonsTdClassName: "route-buttons"
    });
})(angular.module("distanceBtwCities"));