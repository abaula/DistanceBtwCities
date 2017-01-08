(function (ng, app)
{
    "use strict";
    app.controller("overlayController", ["$scope", "overlayService", "domHelperService", "contentLayoutService",
        function ($scope, overlayService, domHelperService, contentLayoutService)
    {
        $scope.overlayVisible = false;
        overlayService.subscribeOnShowOverlay(function ()
        {
            var layoutRect = contentLayoutService.getLayoutRect();
            var overlayContainerElement = domHelperService.getDomElement("#page-overlay-container");
            contentLayoutService.setElementLayout(overlayContainerElement, layoutRect);
            $scope.overlayVisible = true;
        });

        overlayService.subscribeOnHideOverlay(function ()
        {
            $scope.overlayVisible = false;
        });
    }]);
})(angular, angular.module("distanceBtwCities"));