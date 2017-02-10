(function (ng, app)
{
    "use strict";
    app.service("windowResizeWatchService", ["$window", function ($window)
    {
        var windowResizeEventHandlers = [];

        var subscribeOnWindowResize = function (handler)
        {
            windowResizeEventHandlers.push(handler);
        }

        var onWindowResize = function ()
        {
            ng.forEach(windowResizeEventHandlers, function (handler)
            {
                handler();
            });
        }

        ng.element($window).on("resize", onWindowResize);

        return {
            subscribeOnWindowResize: subscribeOnWindowResize
        };
    }]);
})(angular, angular.module("distanceBtwCities"));