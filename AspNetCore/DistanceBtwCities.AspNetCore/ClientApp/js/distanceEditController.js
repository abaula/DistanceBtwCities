(function (ng, app)
{
    "use strict";
    app.controller("distanceEditController", ["$scope", "appConstants", "distanceEditService", "domHelperService", "contentLayoutService",
        "windowResizeWatchService",
        function ($scope, appConstants, distanceEditService, domHelperService, contentLayoutService, windowResizeWatchService)
    {
        var _editRouteItem = undefined;
        $scope.editValue = undefined;
        $scope.editorVisible = false;

        var setLayout = function()
        {
            var layoutRect = contentLayoutService.getOverlayRect();
            var editorControllerElement = domHelperService.getDomElement("#page-distance-editor-controller");
            contentLayoutService.setElementLayout(editorControllerElement, layoutRect);
        }

        windowResizeWatchService.subscribeOnWindowResize(function ()
        {
            if ($scope.editorVisible === false)
                return;

            // Обновляем layout контроллера.
            setLayout();
        });

        distanceEditService.subscribeOnShowEditor(function (routeItem)
        {
            _editRouteItem = routeItem;

            // Размещаем слой формы редактирования.
            setLayout();

            $scope.editValue = _editRouteItem.distance;
            $scope.editorVisible = true;
        });

        $scope.save = function ()
        {

        }

        $scope.close = function ()
        {
            _editRouteItem = undefined;
            $scope.editValue = undefined;
            $scope.editorVisible = false;
        }
    }]);
})(angular, angular.module("distanceBtwCities"));