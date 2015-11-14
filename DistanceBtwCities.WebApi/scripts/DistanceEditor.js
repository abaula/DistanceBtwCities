///<reference path="typings\jquery\jquery.d.ts"/>
///<reference path="ServerAjaxData.ts"/>
///<reference path="CitySelector.ts"/>
var DistanceEditor;
(function (DistanceEditor) {
    var DistanceEditorController = (function () {
        function DistanceEditorController() {
        }
        DistanceEditorController.prototype.onSearchButtonClick = function (event) {
        };
        DistanceEditorController.prototype.applySearchCityData = function () {
            var city = DistanceEditor.__currDistanceEditor.selectedSearchCity;
            var fullName = "";
            if (null != city)
                fullName = city.Fullname;
            $("#i-page-search-form-city-txt").val(fullName);
        };
        DistanceEditorController.prototype.clearSearchCityData = function () {
            $("#i-page-search-form-city-txt").val('');
        };
        DistanceEditorController.prototype.onCitySelected = function (city) {
            DistanceEditor.__currDistanceEditor.selectedSearchCity = city;
            DistanceEditor.__currDistanceEditor.applySearchCityData();
        };
        DistanceEditorController.prototype.onCitySelectedAbort = function () {
            DistanceEditor.__currDistanceEditor.applySearchCityData();
        };
        DistanceEditorController.prototype.onCitySearchTxtFocus = function (event) {
            DistanceEditor.__currDistanceEditor.clearSearchCityData();
        };
        DistanceEditorController.prototype.onDocumentReady = function () {
            /////////////////////////////////////
            // цепляем обработчики событий
            $("#i-page-search-form-city-txt").focus(DistanceEditor.__currDistanceEditor.onCitySearchTxtFocus);
            // подключаем контрол выбора города
            CitySelector.__currentCitySelector.init($("#i-page-search-form-city-txt"), DistanceEditor.__currDistanceEditor);
            // навигация по разделам профайла
            //$("#i-ctrl-profile-navigation-block > div").click(__currentComp.onProfileNavigationMenuItemClick);
            //$("#i-ctrl-profile-update-btn").click(__currentComp.onUpdateButtonClick);
        };
        return DistanceEditorController;
    })();
    DistanceEditor.DistanceEditorController = DistanceEditorController;
    DistanceEditor.__currDistanceEditor = new DistanceEditorController();
})(DistanceEditor || (DistanceEditor = {}));
// настраиваем обработчик события о готовности страницы
$(document).ready(DistanceEditor.__currDistanceEditor.onDocumentReady);
//# sourceMappingURL=DistanceEditor.js.map