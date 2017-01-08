(function (ng, app)
{
    "use strict";
    app.service("distanceEditService", ["$http", function ($http)
    {
        var showEditorHandlers = [];

        var subscribeOnShowEditor = function (handler)
        {
            showEditorHandlers.push(handler);
        }

        var showEditor = function (routeItem)
        {
            ng.forEach(showEditorHandlers, function (handler)
            {
                handler(routeItem);
            });
        }

        return {
            subscribeOnShowEditor: subscribeOnShowEditor,
            showEditor: showEditor
        };
    }]);
})(angular, angular.module("distanceBtwCities"));