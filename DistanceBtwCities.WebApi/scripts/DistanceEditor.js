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
        };
        DistanceEditorController.prototype.onSearchCityButtonClick = function (event) {
            DistanceEditor.__currDistanceEditor.searchByQuery = false;
            DistanceEditor.__currDistanceEditor.getRoutePageData(1);
        };
        DistanceEditorController.prototype.onSearchTextButtonClick = function (event) {
            DistanceEditor.__currDistanceEditor.searchByQuery = true;
            DistanceEditor.__currDistanceEditor.currentSearchQuery = $("#i-page-search-form-search-text-txt").val().trim();
            DistanceEditor.__currDistanceEditor.queryForEmptyDistance = $("#i-page-search-form-search-empty-distance-chk").is(":checked");
            DistanceEditor.__currDistanceEditor.getRoutePageData(1);
        };
        DistanceEditorController.prototype.onPageNavigationClick = function (event) {
            var ctrl = $(event.delegateTarget);
            //window.console.log("onTaskEditClick " + ctrl.attr("data-id"));
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
            $.ajax({
                type: "GET",
                url: "/api/searchroute/" + queryPath,
                success: DistanceEditor.__currDistanceEditor.onAjaxSearchCitySuccess,
                error: DistanceEditor.__currDistanceEditor.onAjaxSearchCityError
            });
        };
        DistanceEditorController.prototype.onAjaxSearchCityError = function (jqXHR, status, message) {
            //window.console.log("_onAjaxError");
            DistanceEditor.__currDistanceEditor.hideOverlay("#i-page-loading-container");
        };
        DistanceEditorController.prototype.onAjaxSearchCitySuccess = function (data, status, jqXHR) {
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
                width: parent.outerWidth(true),
                height: parent.outerHeight(true)
            });
            var iconWidth = 64; //icon.width() = 0 ???;
            var iconHeight = 64; //icon.height() = 0 ???;
            icon.css({
                top: (parent.height() / 2 - (iconHeight / 2)),
                left: (parent.width() / 2 - (iconWidth / 2))
            });
            //overlay.fadeIn(100);
            overlay.show();
        };
        DistanceEditorController.prototype.hideOverlay = function (overlayId) {
            var overlay = $(overlayId);
            //overlay.fadeOut(100);
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
                    $("td.c-page-main-table-col-city1", rowDataTemplate).text(entry.City1.Fullname);
                    $("td.c-page-main-table-col-city2", rowDataTemplate).text(entry.City2.Fullname);
                    $("td.c-page-main-table-col-distance", rowDataTemplate).text(entry.Distance);
                    rowDataTemplate.appendTo(tableBody);
                }
            }
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
        };
        return DistanceEditorController;
    })();
    DistanceEditor.DistanceEditorController = DistanceEditorController;
    DistanceEditor.__currDistanceEditor = new DistanceEditorController();
})(DistanceEditor || (DistanceEditor = {}));
// настраиваем обработчик события о готовности страницы
$(document).ready(DistanceEditor.__currDistanceEditor.onDocumentReady);
//# sourceMappingURL=DistanceEditor.js.map