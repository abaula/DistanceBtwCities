(function (ng, app)
{
    "use strict";
    app.service("contentLayoutService", ["domHelperService", function (domHelperService)
    {
        var getElementRect = function (element)
        {
            return {
                left: domHelperService.getElementLeft(element),
                top: domHelperService.getElementTop(element),
                width: domHelperService.getElementWidth(element),
                height: domHelperService.getElementHeight(element)
            };
        }

        var getOverlayRect = function ()
        {
            return getElementRect(domHelperService.getDomElement("#page-main-container"));
        }

        var setElementLayout = function (element, layoutRect)
        {
            element.css("left", layoutRect.left + "px");
            element.css("top", layoutRect.top + "px");
            element.css("width", layoutRect.width + "px");
            element.css("height", layoutRect.height + "px");
        }

        return {
            getOverlayRect: getOverlayRect,
            getElementRect: getElementRect,
            setElementLayout: setElementLayout
        };
    }]);
})(angular, angular.module("distanceBtwCities"));