(function (ng, app)
{
    "use strict";
    app.service("overlayService", [function ()
    {
        var showOverlayHandlers = [];
        var hideOverlayHandlers = [];

        var subscribeOnShowOverlay = function (handler)
        {
            showOverlayHandlers.push(handler);
        }

        var subscribeOnHideOverlay = function (handler)
        {
            hideOverlayHandlers.push(handler);
        }

        var showOverlay = function ()
        {
            ng.forEach(showOverlayHandlers, function (handler)
            {
                handler();
            });
        }

        var hideOverlay = function ()
        {
            ng.forEach(hideOverlayHandlers, function (handler)
            {
                handler();
            });
        }

        return {
            subscribeOnShowOverlay: subscribeOnShowOverlay,
            subscribeOnHideOverlay: subscribeOnHideOverlay,
            showOverlay: showOverlay,
            hideOverlay: hideOverlay
        };
    }]);
})(angular, angular.module("distanceBtwCities"));