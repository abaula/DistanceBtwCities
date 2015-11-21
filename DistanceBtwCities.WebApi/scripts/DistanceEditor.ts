///<reference path="typings\jquery\jquery.d.ts"/>
///<reference path="ServerAjaxData.ts"/>
///<reference path="CitySelector.ts"/>

module DistanceEditor
{
    export class DistanceEditorController implements CitySelector.ICitySelector
    {
        currentDataFrame: ServerData.AjaxRoutesInfoPackage;
        searchByQuery: boolean;
        selectedSearchCity: ServerData.AjaxCityInfo;
        currentSearchQuery: string;
        queryForEmptyDistance: boolean;
        currentPage: number = 0;
        rowsPerPage: number = 20;

        maxDistance: number = 100000;


        onSearchButtonClick(event: JQueryEventObject): void 
        {
        }

        applySearchCityData(): void
        {
            var searchCityButton: JQuery = $("#i-page-search-form-search-city-btn");
            var city: ServerData.AjaxCityInfo = __currDistanceEditor.selectedSearchCity;
            var fullName: string = "";
            searchCityButton.attr("disabled", "disabled");

            if (null != city)
            {
                fullName = city.Fullname;
                searchCityButton.removeAttr("disabled");
            }

            $("#i-page-search-form-search-city-txt").val(fullName);
        }

        clearSearchCityData(): void 
        {
            $("#i-page-search-form-search-city-txt").val('');
        }

        onCitySelected(city: ServerData.AjaxCityInfo): void 
        {
            __currDistanceEditor.selectedSearchCity = city;
            __currDistanceEditor.applySearchCityData();
        }

        onCitySelectedAbort(): void 
        {
           __currDistanceEditor.applySearchCityData();
        }

        onCitySearchTxtFocus(event: JQueryEventObject): void
        {
            __currDistanceEditor.clearSearchCityData();
        }

        onFormTabClick(event: JQueryEventObject): void
        {
            var ctrl: JQuery = $(event.delegateTarget);

            // если вкладка уже активна, то ничего не делаем
            if (ctrl.hasClass("c-page-search-form-tab-selected"))
                return;

            // меняем активную вкладку
            var id: string = ctrl.attr("id");

            if ("i-page-search-form-tab-search-text" == id)
            {
                $("#i-page-search-form-tab-search-text").addClass("c-page-search-form-tab-selected");
                $("#i-page-search-form-tab-search-city").removeClass("c-page-search-form-tab-selected");

                $("#i-page-search-form-search-city").addClass("hidden");
                $("#i-page-search-form-search-text").removeClass("hidden");

            }
            else if ("i-page-search-form-tab-search-city" == id)
            {
                $("#i-page-search-form-tab-search-city").addClass("c-page-search-form-tab-selected");
                $("#i-page-search-form-tab-search-text").removeClass("c-page-search-form-tab-selected");

                $("#i-page-search-form-search-text").addClass("hidden");
                $("#i-page-search-form-search-city").removeClass("hidden");
            }
        }

        onSearchCityButtonClick(event: JQueryEventObject): void
        {
            __currDistanceEditor.searchByQuery = false;
            __currDistanceEditor.getRoutePageData(1);
        }

        onSearchTextButtonClick(event: JQueryEventObject): void
        {
            __currDistanceEditor.searchByQuery = true;
            __currDistanceEditor.currentSearchQuery = $("#i-page-search-form-search-text-txt").val().trim();
            __currDistanceEditor.queryForEmptyDistance = $("#i-page-search-form-search-empty-distance-chk").is(":checked");
            __currDistanceEditor.getRoutePageData(1);
        }

        onPageNavigationClick(event: JQueryEventObject): void
        {
            var ctrl: JQuery = $(event.delegateTarget);
            //window.console.log("onTaskEditClick " + ctrl.attr("data-id"));

            var pageNum: number = parseInt(ctrl.attr("data-page-num"));

            // загружаем данные указанной страницы
            __currDistanceEditor.getRoutePageData(pageNum);
        }

        getRoutePageData(pageNumber: number): void
        {
            __currDistanceEditor.showOverlay("#i-page-loading-container", "#i-page-main-container");
            __currDistanceEditor.currentPage = pageNumber;

            var prefix: string = __currDistanceEditor.searchByQuery ? "query" : "city";
            var query = __currDistanceEditor.searchByQuery ? __currDistanceEditor.currentSearchQuery : __currDistanceEditor.selectedSearchCity.Id.toString();
            var maxDistance: number = (__currDistanceEditor.searchByQuery && __currDistanceEditor.queryForEmptyDistance) ? 0 : __currDistanceEditor.maxDistance;
            var offset: number = (pageNumber - 1) * __currDistanceEditor.rowsPerPage;

            var queryPath: string = prefix;
            queryPath += "/" + query;
            queryPath += "/" + maxDistance.toString();
            queryPath += "/" + offset.toString();
            queryPath += "/" + __currDistanceEditor.rowsPerPage.toString();

            $.ajax({
                type: "GET",
                url: "/api/searchroute/" + queryPath,
                success: __currDistanceEditor.onAjaxSearchCitySuccess,
                error: __currDistanceEditor.onAjaxSearchCityError
            });

        }

        onAjaxSearchCityError(jqXHR: JQueryXHR, status: string, message: string): void
        {
            //window.console.log("_onAjaxError");
            __currDistanceEditor.hideOverlay("#i-page-loading-container");
        }

        onAjaxSearchCitySuccess(data: ServerData.AjaxRoutesInfoPackage, status: string, jqXHR: JQueryXHR): void
        {
            __currDistanceEditor.currentDataFrame = data;
            __currDistanceEditor.drawTableData();
            __currDistanceEditor.drawPageNavigation();
            __currDistanceEditor.hideOverlay("#i-page-loading-container");
        }


        showOverlay(overlayId: string, parentId: string): void
        {
            var overlay: JQuery = $(overlayId);
            var parent: JQuery = $(parentId);
            var icon: JQuery = $("div.fa-spinner", overlay);

            overlay.css({
                top: 0,
                width: parent.outerWidth(true),
                height: parent.outerHeight(true)
            });

            var iconWidth: number = 64; //icon.width() = 0 ???;
            var iconHeight: number = 64; //icon.height() = 0 ???;

            icon.css({
                top: (parent.height() / 2 - (iconHeight / 2)),
                left: (parent.width() / 2 - (iconWidth / 2))
            });

            //overlay.fadeIn(100);
            overlay.show();
        }

        hideOverlay(overlayId: string): void
        {
            var overlay: JQuery = $(overlayId);
            //overlay.fadeOut(100);
            overlay.hide();
        }

        drawTableData(): void
        {
            __currDistanceEditor.clearTableData();

            var tableBody: JQuery = $("#i-page-main-table > tbody");

            var data: ServerData.AjaxRoutesInfoPackage = __currDistanceEditor.currentDataFrame;

            if (1 > data.Routes.length)
            {
                var rowNoDataTemplate: JQuery = $("#i-page-main-table-row-no-data-template").clone();
                rowNoDataTemplate.removeClass("hidden");
                rowNoDataTemplate.appendTo(tableBody);
            }
            else
            {
                for (var i: number = 0; i < data.Routes.length; i++)
                {
                    var entry: ServerData.AjaxRouteInfo = data.Routes[i];
                    var rowDataTemplate: JQuery = $("#i-page-main-table-row-template").clone();
                    rowDataTemplate.removeClass("hidden");

                    rowDataTemplate.attr("data-route-id", entry.Id);
                    $("td.c-page-main-table-col-city1", rowDataTemplate).text(entry.City1.Fullname);
                    $("td.c-page-main-table-col-city2", rowDataTemplate).text(entry.City2.Fullname);
                    $("td.c-page-main-table-col-distance", rowDataTemplate).text(entry.Distance);




                    rowDataTemplate.appendTo(tableBody);
                }
            }
        }

        clearTableData(): void
        {
            var rows: JQuery = $("#i-page-main-table > tbody > tr");
            rows.remove();
        }

        drawPageNavigation(): void
        {
            __currDistanceEditor.clearPageNavigation();

            // рассчитываем данные панели навигации
            // ... отображаемое количество ссылок
            var displayPagesNumber: number = 10;
            // ... номер страницы в первой отображаемой ссылке
            var displayFirstPageNumber: number;
            // ... номер страницы в последней отображаемой ссылке
            var displayLastPageNumber: number;
            // ... общее количество страниц в выборке
            var allPagesNumber: number;
            // ... номер текущей страницы
            var currentPageNumber: number;

            // рассчитываем общее количество страниц в выборке
            allPagesNumber = Math.ceil(__currDistanceEditor.currentDataFrame.AllFoundRoutesCount / __currDistanceEditor.rowsPerPage);

            // номер текущей страницы
            currentPageNumber = __currDistanceEditor.currentPage;

            // находим номер первой страницы в списке
            displayFirstPageNumber = currentPageNumber - Math.floor(displayPagesNumber / 2);
            displayFirstPageNumber = Math.max(displayFirstPageNumber, 1);
            // находим номер последней страницы в списке
            displayLastPageNumber = Math.min(allPagesNumber, displayFirstPageNumber + displayPagesNumber - 1);
            // корректируем номер первой страницы в списке
            displayFirstPageNumber = Math.max(displayLastPageNumber - displayPagesNumber + 1, 1);

            // рисуем панель навигации
            var container: JQuery = $("#i-page-navpage-container");
            var pageNumTempl: JQuery = $("#i-page-navpage-pagelink-template");

            for (var i: number = displayFirstPageNumber; i <= displayLastPageNumber; i++)
            {
                var pageNumContainer: JQuery = pageNumTempl.clone();
                pageNumContainer.removeAttr("id");
                pageNumContainer.attr("data-page-num", i);
                pageNumContainer.text(i);
                pageNumContainer.appendTo(container);

                if (i == currentPageNumber)
                {
                    pageNumContainer.addClass("c-page-navpage-panel-link-current");
                }
                else
                {
                    // привязываем обработчик события
                    pageNumContainer.click(__currDistanceEditor.onPageNavigationClick);
                }
            }

            // расставляем обработчики и стили на сслыки быстрой навигации
            // ... отображаем ли ссылку на страницу назад
            var displayPrevLinkActive: boolean = (currentPageNumber > 1);
            // ... отображаем ли ссылку на страницу вперёд
            var displayNextLinkActive: boolean = (currentPageNumber < allPagesNumber);
            // ... отображаем ли ссылку на страницу назад
            var displayPrevPageLinkActive: boolean = (displayFirstPageNumber > 1);
            // ... отображаем ли ссылку на страницу вперёд
            var displayNextPageLinkActive: boolean = (displayLastPageNumber < allPagesNumber);
            // ... отображаем ли ссылку на первую страницу
            var displayFirstLinkActive: boolean = (currentPageNumber != 1);
            // ... отображаем ли ссылку на последнюю страницу
            var displayLastLinkActive: boolean = (currentPageNumber < allPagesNumber);

            var linkPageNumber: number;

            if (displayPrevLinkActive)
            {
                linkPageNumber = Math.max(currentPageNumber - 1, 1);
                $("#i-page-navpage-prev").click(__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }

            if (displayNextLinkActive)
            {
                linkPageNumber = Math.min(currentPageNumber + 1, allPagesNumber);
                $("#i-page-navpage-next").click(__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }

            if (displayPrevPageLinkActive)
            {
                linkPageNumber = Math.max(currentPageNumber - displayPagesNumber, 1);
                $("#i-page-navpage-prev-page").click(__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }

            if (displayNextPageLinkActive)
            {
                linkPageNumber = Math.min(currentPageNumber + displayPagesNumber, allPagesNumber);
                $("#i-page-navpage-next-page").click(__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }

            if (displayFirstLinkActive)
            {
                linkPageNumber = 1;
                $("#i-page-navpage-first").click(__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }

            if (displayLastLinkActive)
            {
                linkPageNumber = allPagesNumber;
                $("#i-page-navpage-last").click(__currDistanceEditor.onPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }


        }

        clearPageNavigation(): void
        {
            // удаляем обработчики событий со ссылок быстрой навигации и ставим стили по умолчанию
            $("#i-page-navpage-prev, #i-page-navpage-next, #i-page-navpage-prev-page, #i-page-navpage-next-page, #i-page-navpage-first, #i-page-navpage-last").unbind("click", __currDistanceEditor.onPageNavigationClick).addClass("c-page-navpage-panel-link-disabled").attr("data-page-num", "");
            
            // получаем коллекцию контролов с номерами страниц
            var divs: JQuery = $("#i-page-navpage-container > div");
            // удаляем все обработчики событий
            divs.unbind("click", __currDistanceEditor.onPageNavigationClick);
            // удаляем все контейнеры навигации
            divs.remove();
        }

        onDocumentReady(): void
        {
            /////////////////////////////////////
            // цепляем обработчики событий
            $("#i-page-search-form-search-city-txt").focus(__currDistanceEditor.onCitySearchTxtFocus);

            // подключаем контрол выбора города
            CitySelector.__currentCitySelector.init($("#i-page-search-form-search-city-txt"), __currDistanceEditor);

            // навигация по закладкам
            $("#i-page-search-form-tab-search-text, #i-page-search-form-tab-search-city").click(__currDistanceEditor.onFormTabClick);

            // поиск
            $("#i-page-search-form-search-city-btn").click(__currDistanceEditor.onSearchCityButtonClick);
            $("#i-page-search-form-search-text-btn").click(__currDistanceEditor.onSearchTextButtonClick);

        }
    }  
    
    export var __currDistanceEditor: DistanceEditorController = new DistanceEditorController();       
}

// настраиваем обработчик события о готовности страницы
$(document).ready(DistanceEditor.__currDistanceEditor.onDocumentReady);
