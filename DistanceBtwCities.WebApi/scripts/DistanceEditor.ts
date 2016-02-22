/// <reference path="typings\jquery\jquery.d.ts" />
/// <reference path="ServerAjaxData.ts" />
/// <reference path="CitySelector.ts" />
module DistanceEditor
{
    export class DistanceEditorController implements CitySelector.ICitySelector
    {
        CurrentDataFrame: ServerData.AjaxRoutesInfoPackage;
        SearchByQuery: boolean;
        SelectedSearchCity: ServerData.AjaxCityInfo;
        CurrentSearchQuery: string;
        QueryForEmptyDistance: boolean;
        CurrentPage = 0;
        RowsPerPage = 20;

        MaxDistance = 100000;

        applySearchCityData(): void
        {
            var searchCityButton = $("#i-page-search-form-search-city-btn");
            var city = this.SelectedSearchCity;
            var fullName = "";
            searchCityButton.attr("disabled", "disabled");

            if (null != city)
            {
                fullName = city.Fullname;
                searchCityButton.removeAttr("disabled");
            }

            $("#i-page-search-form-search-city-txt").val(fullName);
            $("#i-page-search-form-search-city-txt").removeClass("c-page-city-select-txt-empty");
        }

        clearSearchCityData(): void
        {
            $("#i-page-search-form-search-city-txt").val("");
            $("#i-page-search-form-search-city-txt").removeClass("c-page-city-select-txt-empty");
        }

        onCitySelected = (city: ServerData.AjaxCityInfo): void =>
        {
            this.SelectedSearchCity = city;
            this.applySearchCityData();
        };

        onCitySelectedAbort = (): void =>
        {
            this.applySearchCityData();
        };

        onCityEmpty = (): void =>
        {
            $("#i-page-search-form-search-city-txt").addClass("c-page-city-select-txt-empty");
        };

        OnCitySearchTxtFocus = (event: JQueryEventObject): void =>
        {
            this.clearSearchCityData();
        };

        OnFormTabClick = (event: JQueryEventObject): void =>
        {
            var ctrl = $(event.delegateTarget);

            // если вкладка уже активна, то ничего не делаем
            if (ctrl.hasClass("c-page-search-form-tab-selected"))
                return;

            // меняем активную вкладку
            var id = ctrl.attr("id");

            if ("i-page-search-form-tab-search-text" === id)
            {
                $("#i-page-search-form-tab-search-text").addClass("c-page-search-form-tab-selected");
                $("#i-page-search-form-tab-search-city").removeClass("c-page-search-form-tab-selected");

                $("#i-page-search-form-search-city").addClass("hidden");
                $("#i-page-search-form-search-text").removeClass("hidden");
            } else if ("i-page-search-form-tab-search-city" === id)
            {
                $("#i-page-search-form-tab-search-city").addClass("c-page-search-form-tab-selected");
                $("#i-page-search-form-tab-search-text").removeClass("c-page-search-form-tab-selected");

                $("#i-page-search-form-search-text").addClass("hidden");
                $("#i-page-search-form-search-city").removeClass("hidden");
            }
        };

        OnSearchCityButtonClick = (event: JQueryEventObject): void =>
        {
            this.SearchByQuery = false;
            this.setSearchParametersPanel();
            this.getRoutePageData(1);
        };

        OnSearchTextButtonClick = (event: JQueryEventObject): void =>
        {
            this.SearchByQuery = true;
            this.CurrentSearchQuery = $("#i-page-search-form-search-text-txt").val().trim();
            this.QueryForEmptyDistance = $("#i-page-search-form-search-empty-distance-chk").is(":checked");
            this.setSearchParametersPanel();
            this.getRoutePageData(1);
        };

        setSearchParametersPanel(): void
        {
            $("#i-page-search-parameters").removeClass("hidden");

            if (this.SearchByQuery)
            {
                $("#i-page-search-parameters-text-value").text(this.CurrentSearchQuery);

                if (this.QueryForEmptyDistance)
                    $("#i-page-search-parameters-text-empty").removeClass("hidden");
                else
                    $("#i-page-search-parameters-text-empty").addClass("hidden");

                $("#i-page-search-parameters-city").addClass("hidden");
                $("#i-page-search-parameters-text").removeClass("hidden");
            } else
            {
                $("#i-page-search-parameters-city-value").text(this.SelectedSearchCity.Fullname);

                $("#i-page-search-parameters-city").removeClass("hidden");
                $("#i-page-search-parameters-text").addClass("hidden");
            }
        }


        OnPageNavigationClick = (event: JQueryEventObject): void =>
        {
            var ctrl = $(event.delegateTarget);

            var pageNum = parseInt(ctrl.attr("data-page-num"));

            // загружаем данные указанной страницы
            this.getRoutePageData(pageNum);
        };

        getRoutePageData(pageNumber: number): void
        {
            this.showOverlay("#i-page-loading-container", "#i-page-main-container");
            this.CurrentPage = pageNumber;

            var prefix = this.SearchByQuery ? "query" : "city";
            var query = this.SearchByQuery ? this.CurrentSearchQuery : this.SelectedSearchCity.Id.toString();
            var maxDistance = (this.SearchByQuery && this.QueryForEmptyDistance) ? 0 : this.MaxDistance;
            var offset = (pageNumber - 1) * this.RowsPerPage;

            var queryPath = prefix;
            queryPath += `/${query}`;
            queryPath += `/${maxDistance.toString()}`;
            queryPath += `/${offset.toString()}`;
            queryPath += `/${this.RowsPerPage.toString()}`;

            // отправляем запрос на сервер
            $.ajax({
                type: "GET",
                url: `/api/searchroute/${queryPath}`,
                success: this.OnAjaxRoutePageDataSuccess,
                error: this.OnAjaxRoutePageDataError
            });
        }

        OnAjaxRoutePageDataError = (jqXhr: JQueryXHR, status: string, message: string): void =>
        {
            // ничего не делаем
            this.hideOverlay("#i-page-loading-container");
        };

        OnAjaxRoutePageDataSuccess = (data: ServerData.AjaxRoutesInfoPackage, status: string, jqXhr: JQueryXHR): void =>
        {
            this.CurrentDataFrame = data;
            this.drawTableData();
            this.drawPageNavigation();
            this.hideOverlay("#i-page-loading-container");
        };

        showOverlay(overlayId: string, parentId: string): void
        {
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
        }

        hideOverlay(overlayId: string): void
        {
            var overlay = $(overlayId);
            overlay.hide();
        }

        drawTableData(): void
        {
            this.clearTableData();

            var tableBody = $("#i-page-main-table > tbody");

            var data = this.CurrentDataFrame;

            if (1 > data.Routes.length)
            {
                var rowNoDataTemplate = $("#i-page-main-table-row-no-data-template").clone();
                rowNoDataTemplate.removeClass("hidden");
                rowNoDataTemplate.appendTo(tableBody);
            } else
            {
                for (var i = 0; i < data.Routes.length; i++)
                {
                    var entry = data.Routes[i];
                    var rowDataTemplate = $("#i-page-main-table-row-template").clone();
                    rowDataTemplate.removeClass("hidden");

                    rowDataTemplate.attr("data-route-id", entry.Id);
                    $("td.c-page-main-table-col-num", rowDataTemplate).text(entry.Id.toString());
                    $("td.c-page-main-table-col-city1", rowDataTemplate).text(entry.City1.Fullname);
                    $("td.c-page-main-table-col-city2", rowDataTemplate).text(entry.City2.Fullname);
                    var distance = this.getFormattedDistance(entry.Distance);
                    $("td.c-page-main-table-col-distance", rowDataTemplate).text(distance);

                    // привязываем обработчик открытия формы редактирования
                    $("td.c-page-main-table-col-edit > i", rowDataTemplate).click(this.OnEditDistanceOpenClick);

                    rowDataTemplate.appendTo(tableBody);
                }
            }
        }

        getFormattedDistance(distance: number): string
        {
            if (distance <= 0)
                return "-";

            return distance.toString();
        }


        getRouteInfoFromCurrentDataFarme(routeId: number): ServerData.AjaxRouteInfo
        {
            var route: ServerData.AjaxRouteInfo = null;
            var routes = this.CurrentDataFrame.Routes;

            for (var i = 0; i < routes.length; i++)
            {
                if (routeId === routes[i].Id)
                {
                    route = routes[i];
                    break;
                }
            }

            return route;
        }

        clearTableData(): void
        {
            var rows = $("#i-page-main-table > tbody > tr");
            rows.remove();
        }

        drawPageNavigation(): void
        {
            this.clearPageNavigation();

            // рассчитываем данные панели навигации
            // ... отображаемое количество ссылок
            var displayPagesNumber = 10;
            // ... номер страницы в первой отображаемой ссылке
            var displayFirstPageNumber: number;
            // ... номер страницы в последней отображаемой ссылке
            var displayLastPageNumber: number;
            // ... общее количество страниц в выборке
            var allPagesNumber: number;
            // ... номер текущей страницы
            var currentPageNumber: number;

            // рассчитываем общее количество страниц в выборке
            allPagesNumber = Math.ceil(this.CurrentDataFrame.AllFoundRoutesCount / this.RowsPerPage);

            // номер текущей страницы
            currentPageNumber = this.CurrentPage;

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

            for (var i = displayFirstPageNumber; i <= displayLastPageNumber; i++)
            {
                var pageNumContainer = pageNumTempl.clone();
                pageNumContainer.removeAttr("id");
                pageNumContainer.attr("data-page-num", i);
                pageNumContainer.text(i);
                pageNumContainer.appendTo(container);

                if (i === currentPageNumber)
                {
                    pageNumContainer.addClass("c-page-navpage-panel-link-current");
                } else
                {
                    // привязываем обработчик события
                    pageNumContainer.click(this.OnPageNavigationClick);
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
            var displayFirstLinkActive = (currentPageNumber !== 1);
            // ... отображаем ли ссылку на последнюю страницу
            var displayLastLinkActive = (currentPageNumber < allPagesNumber);

            var linkPageNumber: number;

            if (displayPrevLinkActive)
            {
                linkPageNumber = Math.max(currentPageNumber - 1, 1);
                $("#i-page-navpage-prev").click(this.OnPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }

            if (displayNextLinkActive)
            {
                linkPageNumber = Math.min(currentPageNumber + 1, allPagesNumber);
                $("#i-page-navpage-next").click(this.OnPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }

            if (displayPrevPageLinkActive)
            {
                linkPageNumber = Math.max(currentPageNumber - displayPagesNumber, 1);
                $("#i-page-navpage-prev-page").click(this.OnPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }

            if (displayNextPageLinkActive)
            {
                linkPageNumber = Math.min(currentPageNumber + displayPagesNumber, allPagesNumber);
                $("#i-page-navpage-next-page").click(this.OnPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }

            if (displayFirstLinkActive)
            {
                linkPageNumber = 1;
                $("#i-page-navpage-first").click(this.OnPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }

            if (displayLastLinkActive)
            {
                linkPageNumber = allPagesNumber;
                $("#i-page-navpage-last").click(this.OnPageNavigationClick).removeClass("c-page-navpage-panel-link-disabled").attr("data-page-num", linkPageNumber);
            }
        }

        clearPageNavigation(): void
        {
            // удаляем обработчики событий со ссылок быстрой навигации и ставим стили по умолчанию
            $("#i-page-navpage-prev, #i-page-navpage-next, #i-page-navpage-prev-page, #i-page-navpage-next-page, #i-page-navpage-first, #i-page-navpage-last").unbind("click", this.OnPageNavigationClick).addClass("c-page-navpage-panel-link-disabled").attr("data-page-num", "");

            // получаем коллекцию контролов с номерами страниц
            var divs = $("#i-page-navpage-container > div");
            // удаляем все обработчики событий
            divs.unbind("click", this.OnPageNavigationClick);
            // удаляем все контейнеры навигации
            divs.remove();
        }

        OnEditDistanceOpenClick = (event: JQueryEventObject): void =>
        {
            // заполняем данные на форме
            var tableRow = $(event.delegateTarget).parent().parent();
            var routeId = parseInt(tableRow.attr("data-route-id"));
            var routeInfo = this.getRouteInfoFromCurrentDataFarme(routeId);

            if (null == routeInfo)
                return;

            $("#i-page-distance-edit-form").attr("data-route-id", routeId);
            $("#i-page-distance-edit-form-city-info-1").text(routeInfo.City1.Fullname);
            $("#i-page-distance-edit-form-city-info-2").text(routeInfo.City2.Fullname);
            var distance = this.getFormattedDistance(routeInfo.Distance);
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

        OnEditDistanceCloseClick = (event: JQueryEventObject): void =>
        {
            this.hideEditDistanceForm();
        };

        hideEditDistanceForm(): void
        {
            // прячем форму
            $("#i-page-distance-edit-form").fadeOut(300);
            // прячем подложку формы
            $("#i-page-edit-form-overlay-mask").fadeOut(300);

            // скрываем информацию об ошибке
            $("#i-page-distance-edit-form-error-message").text("");
            $("#i-page-distance-edit-form-error").addClass("hidden");
        }

        OnEditDistanceSaveClick = (event: JQueryEventObject): void =>
        {
            var routeInfo = new ServerData.AjaxRouteInfo();
            routeInfo.Distance = parseInt($("#i-page-distance-edit-form-new-distance").val());
            routeInfo.Id = parseInt($("#i-page-distance-edit-form").attr("data-route-id"));

            // показываем "крутилку"
            this.showOverlay("#i-distance-edit-form-loading-container", "#i-page-distance-edit-form");

            // Обновляем данные дистанции - отправляем запрос на сервер.
            $.ajax({
                type: "PUT",
                url: "/api/editroute/distance",
                data: JSON.stringify(routeInfo),
                contentType: "application/json",
                dataType: "json",
                success: this.OnAjaxUpdateRouteSuccess,
                error: this.OnAjaxUpdateRouteError
            });
        };

        OnAjaxUpdateRouteError = (jqXhr: JQueryXHR, status: string, message: string): void =>
        {
            // отображаем текст ошибки
            $("#i-page-distance-edit-form-error-message").text(message);
            $("#i-page-distance-edit-form-error").removeClass("hidden");

            // прячем крутилку
            this.hideOverlay("#i-distance-edit-form-loading-container");
        };

        OnAjaxUpdateRouteSuccess = (data: ServerData.AjaxRouteInfo, status: string, jqXhr: JQueryXHR): void =>
        {
            // обновляем значение дистанции в сохранённых страничных данных
            var routeInfo = this.getRouteInfoFromCurrentDataFarme(data.Id);
            routeInfo.Distance = data.Distance;

            // обновляем значение дистанции в таблице
            var tableRow = this.getRouteTableRowByRouteId(data.Id);
            var distance = this.getFormattedDistance(data.Distance);
            $("td.c-page-main-table-col-distance", tableRow).text(distance);

            // прячем "крутилку"
            this.hideOverlay("#i-distance-edit-form-loading-container");

            // скрываем форму редактирования дистанции
            this.hideEditDistanceForm();
        };

        getRouteTableRowByRouteId(routeId: number): JQuery
        {
            return $(`#i-page-main-table > tbody > tr[data-route-id=${routeId.toString()}]`);
        }

        OnEditDistanceFocusOut = (event: JQueryEventObject): void =>
        {
            this.checkNewDistanceValue();
        };

        OnEditDistanceKeyUp = (event: JQueryEventObject): void =>
        {
            this.checkNewDistanceValue();
        };

        checkNewDistanceValue(): void
        {
            var saveBtn = $("#i-page-distance-edit-form-save-btn");
            var newDistanceTxt = $("#i-page-distance-edit-form-new-distance");
            var distance = parseInt(newDistanceTxt.val());

            if (isNaN(distance))
            {
                saveBtn.attr("disabled", "disabled");
                newDistanceTxt.addClass("c-page-distance-edit-form-edit-txt-error");
                newDistanceTxt.val("");
            } else
            {
                newDistanceTxt.val(distance.toString());
                newDistanceTxt.removeClass("c-page-distance-edit-form-edit-txt-error");
                saveBtn.removeAttr("disabled");
            }
        }

        OnDocumentReady = (): void =>
        {
            /////////////////////////////////////
            // цепляем обработчики событий
            $("#i-page-search-form-search-city-txt").focus(this.OnCitySearchTxtFocus);

            // подключаем контрол выбора города
            CitySelector.currentCitySelector.init($("#i-page-search-form-search-city-txt"), currDistanceEditor);

            // навигация по закладкам
            $("#i-page-search-form-tab-search-text, #i-page-search-form-tab-search-city").click(this.OnFormTabClick);

            // поиск
            $("#i-page-search-form-search-city-btn").click(this.OnSearchCityButtonClick);
            $("#i-page-search-form-search-text-btn").click(this.OnSearchTextButtonClick);

            // форма редактирования дистанции
            $("#i-page-distance-edit-form-cancel-btn").click(this.OnEditDistanceCloseClick);
            $("#i-page-distance-edit-form-save-btn").click(this.OnEditDistanceSaveClick);
            $("#i-page-distance-edit-form-new-distance").focusout(this.OnEditDistanceFocusOut);
            $("#i-page-distance-edit-form-new-distance").keyup(this.OnEditDistanceKeyUp);
        };
    }

    export var currDistanceEditor = new DistanceEditorController();
}

// подключаем обработчик события о готовности страницы
$(document).ready(DistanceEditor.currDistanceEditor.OnDocumentReady);