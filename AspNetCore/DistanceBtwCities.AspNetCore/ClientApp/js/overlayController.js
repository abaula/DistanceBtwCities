(function (ng, app)
{
    "use strict";
    app.controller("overlayController", ["$scope", "overlayService", function ($scope, overlayService)
    {
        $scope.overlayVisible = false;
        overlayService.subscribeOnShowOverlay(function ()
        {
            var mainContainerElement = ng.element(document.querySelector("#page-main-container"));
            var overlayContainerElement = ng.element(document.querySelector("#page-overlay-container"));
            overlayContainerElement.css("height", getElementHeight(mainContainerElement) + "px");
            $scope.overlayVisible = true;
        });

        overlayService.subscribeOnHideOverlay(function ()
        {
            $scope.overlayVisible = false;
        });

        var getElementHeight = function(element)
        {
            return element.prop("offsetHeight");
        }
    }]);
})(angular, angular.module("distanceBtwCities"));