///<reference path="typings\jquery\jquery.d.ts"/>
///<reference path="ServerAjaxData.ts"/>
///<reference path="CitySelector.ts"/>
var DistanceEditor;
(function (DistanceEditor) {
    var DistanceEditorController = (function () {
        function DistanceEditorController() {
            this.currentPage = 0;
            this.rowsPerPage = 20;
            this.maxDistance = 100000;
        }
        DistanceEditorController.prototype.onSearchButtonClick = function (event) {
        };
        DistanceEditorController.prototype.applySearchCityData = function () {
            var searchCityButton = $("#i-page-search-form-search-city-btn");
            var city = DistanceEditor.__currDistanceEditor.selectedSearchCity;
            var fullName = "";
            searchCityButton.attr("disabled", "disabled");
            if (null != city) {
                fullName = city.Fullname;
                searchCityButton.removeAttr("disabled");
            }
            $("#i-page-search-form-search-city-txt").val(fullName);
            $("#i-page-search-form-search-city-txt").removeClass("c-page-city-select-txt-empty");
        };
        DistanceEditorController.prototype.clearSearchCityData = function () {
            $("#i-page-search-form-search-city-txt").val("");
            $("#i-page-search-form-search-city-txt").removeClass("c-page-city-select-txt-empty");
        };
        DistanceEditorController.prototype.onCitySelected = function (city) {
            DistanceEditor.__currDistanceEditor.selectedSearchCity = city;
            DistanceEditor.__currDistanceEditor.applySearchCityData();
        };
        DistanceEditorController.prototype.onCitySelectedAbort = function () {
            DistanceEditor.__currDistanceEditor.applySearchCityData();
        };
        DistanceEditorController.prototype.onCityEmpty = function () {
            $("#i-page-search-form-search-city-txt").addClass("c-page-city-select-txt-empty");
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
        };
        DistanceEditorController.prototype.onSearchCityButtonClick = function (event) {
            DistanceEditor.__currDistanceEditor.searchByQuery = false;
            DistanceEditor.__currDistanceEditor.setSearchParametersPanel();
            DistanceEditor.__currDistanceEditor.getRoutePageData(1);
        };
        DistanceEditorController.prototype.onSearchTextButtonClick = function (event) {
            DistanceEditor.__currDistanceEditor.searchByQuery = true;
            DistanceEditor.__currDistanceEditor.currentSearchQuery = $("#i-page-search-form-search-text-txt").val().trim();
            DistanceEditor.__currDistanceEditor.queryForEmptyDistance = $("#i-page-search-form-search-empty-distance-chk").is(":checked");
            DistanceEditor.__currDistanceEditor.setSearchParametersPanel();
            DistanceEditor.__currDistanceEditor.getRoutePageData(1);
        };
        DistanceEditorController.prototype.setSearchParametersPanel = function () {
            $("#i-page-search-parameters").removeClass("hidden");
            if (DistanceEditor.__currDistanceEditor.searchByQuery) {
                $("#i-page-search-parameters-text-value").text(DistanceEditor.__currDistanceEditor.currentSearchQuery);
                if (DistanceEditor.__currDistanceEditor.queryForEmptyDistance)
                    $("#i-page-search-parameters-text-empty").removeClass("hidden");
                else
                    $("#i-page-search-parameters-text-empty").addClass("hidden");
                $("#i-page-search-parameters-city").addClass("hidden");
                $("#i-page-search-parameters-text").removeClass("hidden");
            }
            else {
                $("#i-page-search-parameters-city-value").text(DistanceEditor.__currDistanceEditor.selectedSearchCity.Fullname);
                $("#i-page-search-parameters-city").removeClass("hidden");
                $("#i-page-search-parameters-text").addClass("hidden");
            }
        };
        DistanceEditorController.prototype.onPageNavigationClick = function (event) {
            var ctrl = $(event.delegateTarget);
            var pageNum = parseInt(ctrl.attr("data-page-num"));
            // загружаем данные указанной страницы
            DistanceEditor.__currDistanceEditor.getRoutePageData(pageNum);
        };
        DistanceEditorController.prototype.getRoutePageData = function (pageNumber) {
            DistanceEditor.__currDistanceEditor.showOverlay("#i-page-loading-container", "#i-page-main-container");
            DistanceEditor.__currDistanceEditor.currentPage = pageNumber;
            var prefix = DistanceEditor.__currDistanceEditor.searchByQuery ? "query" : "city";
            var query = DistanceEditor.__currDistanceEditor.searchByQuery ? DistanceEditor.__currDistanceEditor.currentSearchQuery : DistanceEditor.__currDistanceEditor.selectedSearchCity.Id.toString();
            var maxDistance = (DistanceEditor.__currDistanceEditor.searchByQuery && DistanceEditor.__currDistanceEditor.queryForEmptyDistance) ? 0 : DistanceEditor.__currDistanceEditor.maxDistance;
            var offset = (pageNumber - 1) * DistanceEditor.__currDistanceEditor.rowsPerPage;
            var queryPath = prefix;
            queryPath += "/" + query;
            queryPath += "/" + maxDistance.toString();
            queryPath += "/" + offset.toString();
            queryPath += "/" + DistanceEditor.__currDistanceEditor.rowsPerPage.toString();
            // отправляем запрос на сервер
            $.ajax({
                type: "GET",
                url: "/api/searchroute/" + queryPath,
                success: DistanceEditor.__currDistanceEditor.onAjaxRoutePageDataSuccess,
                error: DistanceEditor.__currDistanceEditor.onAjaxRoutePageDataError
            });
        };
        DistanceEditorController.prototype.onAjaxRoutePageDataError = function (jqXHR, status, message) {
            // ничего не делаем
            DistanceEditor.__currDistanceEditor.hideOverlay("#i-page-loading-container");
        };
        DistanceEditorController.prototype.onAjaxRoutePageDataSuccess = function (data, status, jqXHR) {
            DistanceEditor.__currDistanceEditor.currentDataFrame = data;
            DistanceEditor.__currDistanceEditor.drawTableData();
            DistanceEditor.__currDistanceEditor.drawPageNavigation();
            DistanceEditor.__currDistanceEditor.hideOverlay("#i-page-loading-container");
        };
        DistanceEditorController.prototype.showOverlay = function (overlayId, parentId) {
            var overlay = $(overlayId);
            var parent = $(parentId);
            var icon = $("div.fa-spinner", overlay);
            overlay.css({
                top: 0,
                width: parent.innerWidth(),
                height: parent.innerHeight()
            });
            var iconWidth = 64;
            var iconHeight = 64;
            icon.css({
                top: (parent.height() / 2 - (iconHeight / 2)),
                left: (parent.width() / 2 - (iconWidth / 2))
            });
            overlay.show();
        };
        DistanceEditorController.prototype.hideOverlay = function (overlayId) {
            var overlay = $(overlayId);
            overlay.hide();
        };
        DistanceEditorController.prototype.drawTableData = function () {
            DistanceEditor.__currDistanceEditor.clearTableData();
            var tableBody = $("#i-page-main-table > tbody");
            var data = DistanceEditor.__currDistanceEditor.currentDataFrame;
            if (1 > data.Routes.length) {
                var rowNoDataTemplate = $("#i-page-main-table-row-no-data-template").clone();
                rowNoDataTemplate.removeClass("hidden");
                rowNoDataTemplate.appendTo(tableBody);
            }
            else {
                for (var i = 0; i < data.Routes.length; i++) {
                    var entry = data.Routes[i];
                    var rowDataTemplate = $("#i-page-main-table-row-template").clone();
                    rowDataTemplate.removeClass("hidden");
                    rowDataTemplate.attr("data-route-id", entry.Id);
                    $("td.c-page-main-table-col-num", rowDataTemplate).text(entry.Id.toString());
                    $("td.c-page-main-table-col-city1", rowDataTemplate).text(entry.City1.Fullname);
                    $("td.c-page-main-table-col-city2", rowDataTemplate).text(entry.City2.Fullname);
                    var distance = DistanceEditor.__currDistanceEditor.getFormattedDistance(entry.Distance);
                    $("td.c-page-main-table-col-distance", rowDataTemplate).text(distance);
                    // привязываем обработчик открытия формы редактирования
                    $("td.c-page-main-table-col-edit > i", rowDataTemplate).click(DistanceEditor.__currDistanceEditor.onEditDistanceOpenClick);
                    rowDataTemplate.appendTo(tableBody);
                }
            }
        };
        DistanceEditorController.prototype.getFormattedDistance = function (distance) {
            if (distance <= 0)
                return "-";
            return distance.toString();
        };
        DistanceEditorController.prototype.getRouteInfoFromCurrentDataFarme = function (routeId) {
            var route = null;
            var routes = DistanceEditor.__currDistanceEditor.currentDataFrame.Routes;
            for (var i = 0; i < routes.length; i++) {
                if (routeId == routes[i].Id) {
                    route = routes[i];
                    break;
                }
            }
            return route;
        };
        DistanceEditorController.prototype.clearTableData = function () {
            var rows = $("#i-page-main-table > tbody > tr");
            rows.remove();
        };
        DistanceEditorController.prototype.drawPageNavigation = function () {
            DistanceEditor.__currDistanceEditor.clearPageNavigation();
            // рассчитываем данные панели навигации
            // ... отображаемое количество ссылок
            var displayPagesNumber = 10;
            // ... номер страницы в первой отображаемой ссылке
            var displayFirstPageNumber;
            // ... номер страницы в последней отображаемой ссылке
            var displayLastPageNumber;
            // ... общее количество страниц в выборке
            var allPagesNumber;
            // ... номер текущей страницы
            var currentPageNumber;
            // рассчитываем общее количество страниц в выборке
            allPagesNumber = Math.ceil(DistanceEditor.__currDistanceEditor.currentDataFrame.AllFoundRoutesCount / DistanceEditor.__currDistanceEditor.rowsPerPage);
            // номер текущей страницы
            currentPageNumber = DistanceEditor.__currDistanceEditor.currentPage;
            // находим номер первой страницы в списке
            displayFirstPageNumber = currentPageNumber - Math.floor(displayPagesNumber / 2);
            displayFirstPageNumber = Math.max(displayFirstPageNumber, 1);
            // находим номер последней страницы в списке
            displayLastPageNumber = Math.min(allPagesNumber, displayFirstPageNumber + displayPagesNumber - 1);
            // корректируем номер первой страницы в списке
            displayFirstPageNumber = Math.max(displayLastPageNumber - displayPagesNumber + 1, 1);
            // рисуем панель навигации
            var container = $("#i-page-navpage-container");
            var pageNumTempl = $("#i-page-navpage-pagelink-template");
            for (var i = displayFirstPageNumber; i <= displayLastPageNumber; i++) {
                var pageNumContainer = pageNumTempl.clone();
                pageNumContainer.removeAttr("id");
                pageNumContainer.attr("data-page-num", i);
                pageNumContainer.text(i);
                pageNumContainer.appendTo(container);
                if (i == currentPageNumber) {
                    pageNumContainer.addClass("c-page-navpage-panel-link-current");
                }
                else {
                    // привязываем обработчик события
                    pageNumContainer.click(DistanceEditor.__currDistanceEditor.onPageNavigationClick);
                }
            }
            // расставляем обработчики и стили на сслыки быстрой навигации
            // ... отображаем ли ссылку на страницу назад
            var displayPrevLinkActive = (currentPageNumber > 1);
            // ... отображаем ли ссылку на страницу вперёд
            var displayNextLinkActive = (currentPageNumber < allPagesNumber);
            // ... отображаем ли ссылку на страницу назад
            var displayPrevPageLinkActive = (displayFirstPageNumber > 1);
            // ... отображаем ли ссылку на страницу вперёд
            var displayNextPageLinkActive = (displayLastPageNumber < allPagesNumber);
            // ... отображаем ли ссылку на первую страницу
            var displayFirstLinkActive = (currentPageNumber != 1);
            // ... отображаем ли ссылку на последнюю страницу
            var displayLastLinkActive = (currentPageNumber < allPagesNumber);
            var linkPageNumber;
            if (displayPrevLinkActive) {
                linkPageNumber = Math.max(currentPageNumber - 1, 1);
                $("#i-page-navpage-prev").click(DistanceEditor.__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }
            if (displayNextLinkActive) {
                linkPageNumber = Math.min(currentPageNumber + 1, allPagesNumber);
                $("#i-page-navpage-next").click(DistanceEditor.__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }
            if (displayPrevPageLinkActive) {
                linkPageNumber = Math.max(currentPageNumber - displayPagesNumber, 1);
                $("#i-page-navpage-prev-page").click(DistanceEditor.__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }
            if (displayNextPageLinkActive) {
                linkPageNumber = Math.min(currentPageNumber + displayPagesNumber, allPagesNumber);
                $("#i-page-navpage-next-page").click(DistanceEditor.__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }
            if (displayFirstLinkActive) {
                linkPageNumber = 1;
                $("#i-page-navpage-first").click(DistanceEditor.__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }
            if (displayLastLinkActive) {
                linkPageNumber = allPagesNumber;
                $("#i-page-navpage-last").click(DistanceEditor.__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }
        };
        DistanceEditorController.prototype.clearPageNavigation = function () {
            // удаляем обработчики событий со ссылок быстрой навигации и ставим стили по умолчанию
            $("#i-page-navpage-prev, #i-page-navpage-next, #i-page-navpage-prev-page, #i-page-navpage-next-page, #i-page-navpage-first, #i-page-navpage-last").unbind("click", DistanceEditor.__currDistanceEditor.onPageNavigationClick).addClass("c-page-navpage-panel-link-disabled").attr("data-page-num", "");
            // получаем коллекцию контролов с номерами страниц
            var divs = $("#i-page-navpage-container > div");
            // удаляем все обработчики событий
            divs.unbind("click", DistanceEditor.__currDistanceEditor.onPageNavigationClick);
            // удаляем все контейнеры навигации
            divs.remove();
        };
        DistanceEditorController.prototype.onEditDistanceOpenClick = function (event) {
            // заполняем данные на форме
            var tableRow = $(event.delegateTarget).parent().parent();
            var routeId = parseInt(tableRow.attr("data-route-id"));
            var routeInfo = DistanceEditor.__currDistanceEditor.getRouteInfoFromCurrentDataFarme(routeId);
            if (null == routeInfo)
                return;
            $("#i-page-distance-edit-form").attr("data-route-id", routeId);
            $("#i-page-distance-edit-form-city-info-1").text(routeInfo.City1.Fullname);
            $("#i-page-distance-edit-form-city-info-2").text(routeInfo.City2.Fullname);
            var distance = DistanceEditor.__currDistanceEditor.getFormattedDistance(routeInfo.Distance);
            $("#i-page-distance-edit-form-current-distance").text(distance);
            $("#i-page-distance-edit-form-new-distance").val("").removeClass("c-page-distance-edit-form-edit-txt-error");
            $("#i-page-distance-edit-form-save-btn").attr("disabled", "disabled");
            // отображаем подложку формы
            $("#i-page-edit-form-overlay-mask").fadeIn(300);
            // отображаем форму
            var editForm = $("#i-page-distance-edit-form");
            editForm.fadeIn(300);
            // выравниваем позицию формы по центру экрана
            var popTop = editForm.height() / 2;
            var popLeft = editForm.width() / 2;
            editForm.css({
                'margin-top': -popTop,
                'margin-left': -popLeft
            });
            // назначаем фокус ввода для текстового поля
            $("#i-page-distance-edit-form-new-distance").focus();
        };
        DistanceEditorController.prototype.onEditDistanceCloseClick = function (event) {
            DistanceEditor.__currDistanceEditor.hideEditDistanceForm();
        };
        DistanceEditorController.prototype.hideEditDistanceForm = function () {
            // прячем форму
            $("#i-page-distance-edit-form").fadeOut(300);
            // прячем подложку формы
            $("#i-page-edit-form-overlay-mask").fadeOut(300);
            // скрываем информацию об ошибке
            $("#i-page-distance-edit-form-error-message").text("");
            $("#i-page-distance-edit-form-error").addClass("hidden");
        };
        DistanceEditorController.prototype.onEditDistanceSaveClick = function (event) {
            var routeInfo = new ServerData.AjaxRouteInfo();
            routeInfo.Distance = parseInt($("#i-page-distance-edit-form-new-distance").val());
            routeInfo.Id = parseInt($("#i-page-distance-edit-form").attr("data-route-id"));
            // показываем "крутилку"
            DistanceEditor.__currDistanceEditor.showOverlay("#i-distance-edit-form-loading-container", "#i-page-distance-edit-form");
            // Обновляем данные дистанции - отправляем запрос на сервер.
            $.ajax({
                type: "PUT",
                url: "/api/editroute/distance",
                data: JSON.stringify(routeInfo),
                contentType: "application/json",
                dataType: "json",
                success: DistanceEditor.__currDistanceEditor.onAjaxUpdateRouteSuccess,
                error: DistanceEditor.__currDistanceEditor.onAjaxUpdateRouteError
            });
        };
        DistanceEditorController.prototype.onAjaxUpdateRouteError = function (jqXHR, status, message) {
            // отображаем текст ошибки
            $("#i-page-distance-edit-form-error-message").text(message);
            $("#i-page-distance-edit-form-error").removeClass("hidden");
            // прячем крутилку
            DistanceEditor.__currDistanceEditor.hideOverlay("#i-distance-edit-form-loading-container");
        };
        DistanceEditorController.prototype.onAjaxUpdateRouteSuccess = function (data, status, jqXHR) {
            // обновляем значение дистанции в сохранённых страничных данных
            var routeInfo = DistanceEditor.__currDistanceEditor.getRouteInfoFromCurrentDataFarme(data.Id);
            routeInfo.Distance = data.Distance;
            // обновляем значение дистанции в таблице
            var tableRow = DistanceEditor.__currDistanceEditor.getRouteTableRowByRouteId(data.Id);
            var distance = DistanceEditor.__currDistanceEditor.getFormattedDistance(data.Distance);
            $("td.c-page-main-table-col-distance", tableRow).text(distance);
            // прячем "крутилку"
            DistanceEditor.__currDistanceEditor.hideOverlay("#i-distance-edit-form-loading-container");
            // скрываем форму редактирования дистанции
            DistanceEditor.__currDistanceEditor.hideEditDistanceForm();
        };
        DistanceEditorController.prototype.getRouteTableRowByRouteId = function (routeId) {
            return $("#i-page-main-table > tbody > tr[data-route-id=" + routeId.toString() + "]");
        };
        DistanceEditorController.prototype.onEditDistanceFocusOut = function (event) {
            DistanceEditor.__currDistanceEditor.checkNewDistanceValue();
        };
        DistanceEditorController.prototype.onEditDistanceKeyUp = function (event) {
            DistanceEditor.__currDistanceEditor.checkNewDistanceValue();
        };
        DistanceEditorController.prototype.checkNewDistanceValue = function () {
            var saveBtn = $("#i-page-distance-edit-form-save-btn");
            var newDistanceTxt = $("#i-page-distance-edit-form-new-distance");
            var distance = parseInt(newDistanceTxt.val());
            if (isNaN(distance)) {
                saveBtn.attr("disabled", "disabled");
                newDistanceTxt.addClass("c-page-distance-edit-form-edit-txt-error");
                newDistanceTxt.val("");
            }
            else {
                newDistanceTxt.val(distance.toString());
                newDistanceTxt.removeClass("c-page-distance-edit-form-edit-txt-error");
                saveBtn.removeAttr("disabled");
            }
        };
        DistanceEditorController.prototype.onDocumentReady = function () {
            /////////////////////////////////////
            // цепляем обработчики событий
            $("#i-page-search-form-search-city-txt").focus(DistanceEditor.__currDistanceEditor.onCitySearchTxtFocus);
            // подключаем контрол выбора города
            CitySelector.__currentCitySelector.init($("#i-page-search-form-search-city-txt"), DistanceEditor.__currDistanceEditor);
            // навигация по закладкам
            $("#i-page-search-form-tab-search-text, #i-page-search-form-tab-search-city").click(DistanceEditor.__currDistanceEditor.onFormTabClick);
            // поиск
            $("#i-page-search-form-search-city-btn").click(DistanceEditor.__currDistanceEditor.onSearchCityButtonClick);
            $("#i-page-search-form-search-text-btn").click(DistanceEditor.__currDistanceEditor.onSearchTextButtonClick);
            // форма редактирования дистанции
            $("#i-page-distance-edit-form-cancel-btn").click(DistanceEditor.__currDistanceEditor.onEditDistanceCloseClick);
            $("#i-page-distance-edit-form-save-btn").click(DistanceEditor.__currDistanceEditor.onEditDistanceSaveClick);
            $("#i-page-distance-edit-form-new-distance").focusout(DistanceEditor.__currDistanceEditor.onEditDistanceFocusOut);
            $("#i-page-distance-edit-form-new-distance").keyup(DistanceEditor.__currDistanceEditor.onEditDistanceKeyUp);
        };
        return DistanceEditorController;
    })();
    DistanceEditor.DistanceEditorController = DistanceEditorController;
    DistanceEditor.__currDistanceEditor = new DistanceEditorController();
})(DistanceEditor || (DistanceEditor = {}));
// подключаем обработчик события о готовности страницы
$(document).ready(DistanceEditor.__currDistanceEditor.onDocumentReady);
//# sourceMappingURL=DistanceEditor.js.map