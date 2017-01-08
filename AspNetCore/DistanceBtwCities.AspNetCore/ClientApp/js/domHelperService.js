(function (ng, app)
{
    "use strict";
    app.service("domHelperService", [function ()
    {
        var getElementLeft = function (element)
        {
            return element.prop("offsetLeft");
        }

        var getElementTop = function (element)
        {
            return element.prop("offsetTop");
        }

        var getElementWidth = function (element)
        {
            return element.prop("offsetWidth");
        }

        var getElementHeight = function (element)
        {
            return element.prop("offsetHeight");
        }

        var getDomElement = function (query)
        {
            return ng.element(document.querySelector(query));
        }

        return {
            getDomElement: getDomElement,
            getElementLeft: getElementLeft,
            getElementTop: getElementTop,
            getElementWidth: getElementWidth,
            getElementHeight: getElementHeight,
        };
    }]);
})(angular, angular.module("distanceBtwCities"));