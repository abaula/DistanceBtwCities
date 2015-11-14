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
            $("#i-page-search-form-search-city-txt").val(fullName);
        };
        DistanceEditorController.prototype.clearSearchCityData = function () {
            $("#i-page-search-form-search-city-txt").val('');
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
        DistanceEditorController.prototype.onFormTabClick = function (event) {
            var ctrl = $(event.delegateTarget);
            // если вкладка уже активна, то ничего не делаем
            if (ctrl.hasClass("c-page-search-form-tab-selected"))
                return;
            // меняем активную вкладку
            var id = ctrl.attr("id");
            if ("i-page-search-form-tab-search-text" == id) {
                $("#i-page-search-form-tab-search-text").addClass("c-page-search-form-tab-selected");
                $("#i-page-search-form-tab-search-city").removeClass("c-page-search-form-tab-selected");
                $("#i-page-search-form-search-city").addClass("hidden");
                $("#i-page-search-form-search-text").removeClass("hidden");
            }
            else if ("i-page-search-form-tab-search-city" == id) {
                $("#i-page-search-form-tab-search-city").addClass("c-page-search-form-tab-selected");
                $("#i-page-search-form-tab-search-text").removeClass("c-page-search-form-tab-selected");
                $("#i-page-search-form-search-text").addClass("hidden");
                $("#i-page-search-form-search-city").removeClass("hidden");
            }
            //event.delegateTarget.
        };
        DistanceEditorController.prototype.onDocumentReady = function () {
            /////////////////////////////////////
            // цепляем обработчики событий
            $("#i-page-search-form-search-city-txt").focus(DistanceEditor.__currDistanceEditor.onCitySearchTxtFocus);
            // подключаем контрол выбора города
            CitySelector.__currentCitySelector.init($("#i-page-search-form-search-city-txt"), DistanceEditor.__currDistanceEditor);
            // навигация по закладкам
            $("#i-page-search-form-tab-search-text, #i-page-search-form-tab-search-city").click(DistanceEditor.__currDistanceEditor.onFormTabClick);
        };
        return DistanceEditorController;
    })();
    DistanceEditor.DistanceEditorController = DistanceEditorController;
    DistanceEditor.__currDistanceEditor = new DistanceEditorController();
})(DistanceEditor || (DistanceEditor = {}));
// настраиваем обработчик события о готовности страницы
$(document).ready(DistanceEditor.__currDistanceEditor.onDocumentReady);
//# sourceMappingURL=DistanceEditor.js.map