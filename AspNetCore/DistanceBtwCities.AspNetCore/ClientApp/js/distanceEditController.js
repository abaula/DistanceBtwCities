(function (ng, app)
{
    "use strict";
    app.controller("distanceEditController", ["$scope", "appConstants", "distanceEditService", "domHelperService", "contentLayoutService",
        "windowResizeWatchService",
        function ($scope, appConstants, distanceEditService, domHelperService, contentLayoutService, windowResizeWatchService)
    {
        var editRouteItem;

        var setValuesDefault = function ()
        {
            editRouteItem = undefined;
            $scope.editValue = undefined;
            $scope.editorVisible = false;
            $scope.city1Name = "";
            $scope.city2Name = "";
            $scope.currentDistance = 0;
        }

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
            editRouteItem = routeItem;

            // Размещаем слой формы редактирования.
            setLayout();

            $scope.city1Name = editRouteItem.city1.name;
            $scope.city2Name = editRouteItem.city2.name;
            $scope.currentDistance = editRouteItem.distance;
            $scope.editValue = editRouteItem.distance;
            $scope.editorVisible = true;
        });

        distanceEditService.subscribeOnDistanceUpdated(function (routeId, distance)
        {
            setValuesDefault();
            $scope.editorVisible = false;
        });

        $scope.save = function ()
        {
            distanceEditService.updateDistance(editRouteItem.id, $scope.editValue);
        }

        $scope.close = function ()
        {
            setValuesDefault();
            $scope.editorVisible = false;
        }

        setValuesDefault();
    }]);
})(angular, angular.module("distanceBtwCities"));