(function (ng, app)
{
    "use strict";
    app.controller("distanceEditController", ["$scope", "appConstants", "distanceEditService", "domHelperService", "contentLayoutService",
        function ($scope, appConstants, distanceEditService, domHelperService, contentLayoutService)
    {
        var _editRouteItem = undefined;
        $scope.editValue = undefined;
        $scope.editorVisible = false;

        distanceEditService.subscribeOnShowEditor(function (routeItem)
        {
            _editRouteItem = routeItem;
            // Размещаем слой формы редактирования.
            var layoutRect = contentLayoutService.getLayoutRect();
            var mainContainerElement = domHelperService.getDomElement("#page-main-container");
            var editorControllerElement = domHelperService.getDomElement("#page-distance-editor-controller");
            contentLayoutService.setElementLayout(editorControllerElement, layoutRect);

            // Размещаем элементы формы редактирования.
            var routesTableElement = domHelperService.getDomElement("#routes-table");
            var routesTableLeftOffset = domHelperService.getElementLeft(routesTableElement);
            var routesTableTopOffset = domHelperService.getElementTop(routesTableElement);
            var editorControllerLeftOffset = domHelperService.getElementLeft(mainContainerElement);
            var editorControllerTopOffset = domHelperService.getElementTop(mainContainerElement);

            var offsetLeft = routesTableLeftOffset - editorControllerLeftOffset;
            var offsetTop = routesTableTopOffset - editorControllerTopOffset;


            var rowId = "#" + appConstants.dataRowIdPrefix + _editRouteItem.id;
            var targetDistanceCellElement = domHelperService.getDomElement(rowId + " > td." + appConstants.distanceTdClassName);            
            var editValueControlContainer = domHelperService.getDomElement("#page-distance-editor-controller > div.edit-value-control-container");
            editValueControlContainer.css("left", (domHelperService.getElementLeft(targetDistanceCellElement) + offsetLeft) + "px");
            editValueControlContainer.css("top", (domHelperService.getElementTop(targetDistanceCellElement) + offsetTop) + "px");
            editValueControlContainer.css("width", domHelperService.getElementWidth(targetDistanceCellElement) + "px");
            editValueControlContainer.css("height", domHelperService.getElementHeight(targetDistanceCellElement) + "px");


            /*
            console.log(targetDistanceCellElement.html());
            console.log(domHelperService.getElementLeft(targetDistanceCellElement)
                + "-" + domHelperService.getElementTop(targetDistanceCellElement)
                + "-" + domHelperService.getElementWidth(targetDistanceCellElement)
                + "-" + domHelperService.getElementHeight(targetDistanceCellElement)
            )
            */

            var targetButtonsCellElement = domHelperService.getDomElement(rowId + " > td." + appConstants.buttonsTdClassName);
            var editValueButtonsContainer = domHelperService.getDomElement("#page-distance-editor-controller > div.edit-value-buttons-container");
            editValueButtonsContainer.css("left", (domHelperService.getElementLeft(targetButtonsCellElement) + offsetLeft) + "px");
            editValueButtonsContainer.css("top", (domHelperService.getElementTop(targetButtonsCellElement) + offsetTop) + "px");
            editValueButtonsContainer.css("width", domHelperService.getElementWidth(targetButtonsCellElement) + "px");
            editValueButtonsContainer.css("height", domHelperService.getElementHeight(targetButtonsCellElement) + "px");

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