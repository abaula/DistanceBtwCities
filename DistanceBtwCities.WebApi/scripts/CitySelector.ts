///<reference path="typings\jquery\jquery.d.ts"/>
///<reference path="ServerAjaxData.ts"/>

module CitySelector 
{
    export interface ICitySelector
    {
        onCitySelected(city: ServerData.AjaxCityInfo): void;
        onCitySelectedAbort(): void;
    }

    export class CitySelector
    {
        control: JQuery = null;
        component: ICitySelector = null;
        timer: number = 0;
        searchValue: string = null;
        busy: boolean = false;
        timeoutMs: number = 1000;
        currentRequest: JQueryXHR = null;
        listBlockHtml: JQuery = null;
        listData: ServerData.AjaxCityInfo[] = null;

        init(control: JQuery, component: ICitySelector): void
        {
            __currentCitySelector.clear();
            __currentCitySelector.component = component;
            __currentCitySelector.control = control;
            __currentCitySelector.control.bind("cut paste", __currentCitySelector.onTextChange);
            __currentCitySelector.control.bind("keydown", __currentCitySelector.onKeyDown);
            __currentCitySelector.control.bind("blur", __currentCitySelector.onLostFocus);
        }

        clear(): void
        {
            __currentCitySelector.clearListBlock();

            if (0 < __currentCitySelector.timer)
                clearTimeout(__currentCitySelector.timer);

            if (null != __currentCitySelector.control)
            {
                __currentCitySelector.control.unbind("cut paste", __currentCitySelector.onTextChange);
                __currentCitySelector.control.unbind("keydown", __currentCitySelector.onKeyDown);
                __currentCitySelector.control.unbind("blur", __currentCitySelector.onLostFocus);
            }

            // сбрасываем отправленные на сервер запросы
            if (null != __currentCitySelector.currentRequest)
                __currentCitySelector.currentRequest.abort();

            __currentCitySelector.currentRequest = null;
            __currentCitySelector.busy = false;
            __currentCitySelector.listData = null;
            __currentCitySelector.component = null;
        }

        clearListBlock(): void
        {
            if (null != __currentCitySelector.listBlockHtml)
            {
                $("div", __currentCitySelector.listBlockHtml).unbind("click mouseenter mouseleave");
                __currentCitySelector.listBlockHtml.remove();
                __currentCitySelector.listBlockHtml = null;
            }
        }

        onMouseEnter(event: JQueryEventObject): void
        {
            $("div.c-page-city-select-item", __currentCitySelector.listBlockHtml).removeClass("c-page-city-select-item-selected");
            $(event.delegateTarget).addClass("c-page-city-select-item-selected");
        }

        onMouseLeave(event: JQueryEventObject): void
        {
            $("div.c-page-city-select-item", __currentCitySelector.listBlockHtml).removeClass("c-page-city-select-item-selected");
        }

        onKeyDown(event: JQueryEventObject): void
        {
            if (null != __currentCitySelector.listBlockHtml)
            {
                if (40 == event.keyCode)
                // arrow down
                {
                    __currentCitySelector.onArrowDown();
                    return;
                }
                else if (38 == event.keyCode)
                // arrow up
                {
                    __currentCitySelector.onArrowUp();
                    return;
                }
                else if (13 == event.keyCode)
                // enter
                {
                    __currentCitySelector.onEnter();
                    return;
                }
                else if (27 == event.keyCode)
                // escape
                {
                    __currentCitySelector.onEscape();
                    return;
                }
            }

            __currentCitySelector.onTextChange(event);
        }

        onArrowDown(): void
        {
            var selectedItem: JQuery = __currentCitySelector.getSelectedItem();

            if (0 < selectedItem.length)
            {
                var nextItem: JQuery = selectedItem.next();

                if (0 < nextItem.length)
                {
                    selectedItem.removeClass("c-page-city-select-item-selected");
                    nextItem.addClass("c-page-city-select-item-selected");
                }
            }
            else
            {
                $("div.c-page-city-select-item", __currentCitySelector.listBlockHtml).first().addClass("c-page-city-select-item-selected");
            }
        }

        onArrowUp(): void
        {
            var selectedItem: JQuery = __currentCitySelector.getSelectedItem();

            if (0 < selectedItem.length)
            {
                var prevItem: JQuery = selectedItem.prev();

                if (0 < prevItem.length)
                {
                    selectedItem.removeClass("c-page-city-select-item-selected");
                    prevItem.addClass("c-page-city-select-item-selected");
                }
            }
        }

        onEnter(): void
        {
            var selectedItem: JQuery = __currentCitySelector.getSelectedItem();

            if (0 < selectedItem.length)
            {
                var id: number = parseInt(selectedItem.attr("data-id"));
                __currentCitySelector.onItemClicked(id);
            }
        }

        onEscape(): void
        {
            __currentCitySelector.clearListBlock();
        }


        getSelectedItem(): JQuery
        {
            return $("div.c-page-city-select-item-selected", __currentCitySelector.listBlockHtml);
        }

        onTextChange(event: JQueryEventObject): void
        {
            if (0 < __currentCitySelector.timer)
                clearTimeout(__currentCitySelector.timer);

            if (true == __currentCitySelector.busy)
                return;

            __currentCitySelector.timer = setTimeout(__currentCitySelector.onTimeout, __currentCitySelector.timeoutMs);
        }

        onTimeout(): void
        {
            var value: string = __currentCitySelector.control.val().trim();

            if (value == __currentCitySelector.searchValue)
                return;

            __currentCitySelector.searchValue = value;

            if (3 > value.length)
            {
                __currentCitySelector.clearListBlock();
                return;
            }

            //window.console.log(__currentCitySelector.searchValue);
            // выставляем флаг блокировки поиска и отправляем поисковый запрос на сервер
            __currentCitySelector.busy = true;
            __currentCitySelector.getData(value);
        }


        getData(query: string): void
        {
            __currentCitySelector.currentRequest = $.ajax({
                type: "GET",
                url: "/api/searchcity/" + query,
                success: __currentCitySelector.onAjaxGetOrgDataSuccess,
                error: __currentCitySelector.onAjaxGetOrgDataError
            });
        }

        onAjaxGetOrgDataError(jqXHR: JQueryXHR, status: string, message: string): void
        {
            __currentCitySelector.busy = false;
            __currentCitySelector.currentRequest = null;
            //window.console.log("_onAjaxError");
        }

        onAjaxGetOrgDataSuccess(data: ServerData.AjaxCityInfo[], status: string, jqXHR: JQueryXHR): void
        {
            __currentCitySelector.busy = false;
            __currentCitySelector.currentRequest = null;
            //window.console.log("_onAjaxGetAccountDataSuccess");

            __currentCitySelector.listData = data;
            // данные получены рисуем выпадающий список выбора
            __currentCitySelector.drawList();
        }

        drawList(): void
        {
            __currentCitySelector.clearListBlock();
            var data: ServerData.AjaxCityInfo[] = __currentCitySelector.listData;

            // если контрол уже потерял фокус, то ничего не отображаем
            if (false == __currentCitySelector.control.is(":focus"))
                return;

            if (null == data || 1 > data.length)
                return;

            __currentCitySelector.listBlockHtml = $('<div class="c-page-city-select-block"></div>');

            for (var i: number = 0; i < data.length; i++)
            {
                var city: ServerData.AjaxCityInfo = data[i];
                var item: JQuery = $('<div class="c-page-city-select-item"></div>');
                item.text(city.Fullname);
                item.attr("data-id", city.Id);
                item.bind("click", __currentCitySelector.onItemClick);
                item.bind("mouseenter", __currentCitySelector.onMouseEnter);
                item.bind("mouseleave", __currentCitySelector.onMouseLeave);
                __currentCitySelector.listBlockHtml.append(item);
            }

            // вычисляем положение списка на экране
            var top: number = __currentCitySelector.control.offset().top;
            top += __currentCitySelector.control.height();
            var left: number = __currentCitySelector.control.offset().left;

            __currentCitySelector.listBlockHtml.appendTo($("body"));
            __currentCitySelector.listBlockHtml.css({ top: top, left: left });
        }

        onItemClick(event: JQueryEventObject): void
        {
            __currentCitySelector.searchValue = null;
            var elem: JQuery = $(event.delegateTarget);
            //window.console.log("__currentCitySelector.onItemClick(" + elem.attr("data-id") + ")");
            __currentCitySelector.onItemClicked(parseInt(elem.attr("data-id")));
        }

        onItemClicked(id: number): void
        {
            var city = __currentCitySelector.getCityById(id);
            __currentCitySelector.component.onCitySelected(city);
            __currentCitySelector.clearListBlock();
        }

        getCityById(id: number): ServerData.AjaxCityInfo
        {
            var city: ServerData.AjaxCityInfo = null;

            if (null != __currentCitySelector.listData)
            {
                for (var i: number = 0; i < __currentCitySelector.listData.length; i++)
                {
                    var c: ServerData.AjaxCityInfo = __currentCitySelector.listData[i];

                    if (id == c.Id)
                    {
                        city = c;
                        break;
                    }
                }
            }

            return city;
        }

        onLostFocus(event: JQueryEventObject): void
        {
            if (null != __currentCitySelector.listBlockHtml)
            {
                if ($(__currentCitySelector.listBlockHtml).is(':hover'))
                    setTimeout(__currentCitySelector.clearListBlock, 500);
                else
                    __currentCitySelector.clearListBlock();
            }

            __currentCitySelector.searchValue = null;
            __currentCitySelector.component.onCitySelectedAbort();
        }
    }
    
    export var __currentCitySelector = new CitySelector();
} 