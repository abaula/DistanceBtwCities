(function (ng, app)
{
    "use strict";
    app.controller("overlayController", ["$scope", "overlayService", "domHelperService", "contentLayoutService", "windowResizeWatchService",
        function ($scope, overlayService, domHelperService, contentLayoutService, windowResizeWatchService)
    {
        $scope.overlayVisible = false;

        var layoutOverlay = function ()
        {
            var layoutRect = contentLayoutService.getOverlayRect();
            var overlayContainerElement = domHelperService.getDomElement("#page-overlay-container");
            contentLayoutService.setElementLayout(overlayContainerElement, layoutRect);
        }

        windowResizeWatchService.subscribeOnWindowResize(function ()
        {
            if ($scope.overlayVisible === false)
                return;

            // обновляем layout контроллера
            layoutOverlay();
        });

        overlayService.subscribeOnShowOverlay(function ()
        {
            layoutOverlay();
            $scope.overlayVisible = true;
        });

        overlayService.subscribeOnHideOverlay(function ()
        {
            $scope.overlayVisible = false;
        });
    }]);
})(angular, angular.module("distanceBtwCities"));