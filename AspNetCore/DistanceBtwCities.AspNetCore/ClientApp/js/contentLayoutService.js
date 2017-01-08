(function (ng, app)
{
    "use strict";
    app.service("contentLayoutService", ["domHelperService", function (domHelperService)
    {
        var getLayoutRect = function()
        {
            var mainContainerElement = domHelperService.getDomElement("#page-main-container");
            // Ширину всегда считаем по таблице, т.к. её сжатие по ширине ограничено.
            var routesTableElement = domHelperService.getDomElement("#routes-table");

            return {
                left: domHelperService.getElementLeft(mainContainerElement),
                top: domHelperService.getElementTop(mainContainerElement),
                width: domHelperService.getElementWidth(routesTableElement),
                height: domHelperService.getElementHeight(mainContainerElement)
            };
        }

        var setElementLayout = function(element, layoutRect)
        {
            element.css("left", layoutRect.left + "px");
            element.css("top", layoutRect.top + "px");
            element.css("width", layoutRect.width + "px");
            element.css("height", layoutRect.height + "px");
        }

        return {
            getLayoutRect: getLayoutRect,
            setElementLayout: setElementLayout
        };
    }]);
})(angular, angular.module("distanceBtwCities"));