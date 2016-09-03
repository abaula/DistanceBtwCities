/// <reference path="typings\jquery\jquery.d.ts" />
/// <reference path="ServerAjaxData.ts" />
module CitySelector
{
    export interface ICitySelector
    {
        onCitySelected(city: ServerData.AjaxCityInfo): void;
        onCitySelectedAbort(): void;
        onCityEmpty(): void;
    }

    export class CitySelector
    {
        Control: JQuery = null;
        Component: ICitySelector = null;
        Timer = 0;
        SearchValue: string = null;
        Busy = false;
        TimeoutMs = 1000;
        CurrentRequest: JQueryXHR = null;
        ListBlockHtml: JQuery = null;
        ListData: ServerData.AjaxCityInfo[] = null;


        constructor(control: JQuery, component: ICitySelector)
        {
            this.Component = component;
            this.Control = control;
            this.Control.bind("cut paste", this.OnTextChange);
            this.Control.bind("keydown", this.OnKeyDown);
            this.Control.bind("blur", this.OnLostFocus);
        }

        clearListBlock(): void
        {
            if (null != this.ListBlockHtml)
            {
                $("div", this.ListBlockHtml).unbind("click mouseenter mouseleave");
                this.ListBlockHtml.remove();
                this.ListBlockHtml = null;
            }
        }

        OnMouseEnter = (event: JQueryEventObject): void =>
        {
            $("div.c-page-city-select-item", this.ListBlockHtml).removeClass("c-page-city-select-item-selected");
            $(event.delegateTarget).addClass("c-page-city-select-item-selected");
        };

        OnMouseLeave = (event: JQueryEventObject): void =>
        {
            $("div.c-page-city-select-item", this.ListBlockHtml).removeClass("c-page-city-select-item-selected");
        };

        OnKeyDown = (event: JQueryEventObject): void =>
        {
            if (null != this.ListBlockHtml)
            {
                if (40 === event.keyCode)
                // arrow down
                {
                    this.OnArrowDown();
                    return;
                } else if (38 === event.keyCode)
                // arrow up
                {
                    this.OnArrowUp();
                    return;
                } else if (13 === event.keyCode)
                // enter
                {
                    this.OnEnter();
                    return;
                } else if (27 === event.keyCode)
                // escape
                {
                    this.OnEscape();
                    return;
                }
            }

            this.OnTextChange(event);
        };

        OnArrowDown = (): void =>
        {
            var selectedItem = this.getSelectedItem();

            if (0 < selectedItem.length)
            {
                var nextItem = selectedItem.next();

                if (0 < nextItem.length)
                {
                    selectedItem.removeClass("c-page-city-select-item-selected");
                    nextItem.addClass("c-page-city-select-item-selected");
                }
            } else
            {
                $("div.c-page-city-select-item", this.ListBlockHtml).first().addClass("c-page-city-select-item-selected");
            }
        };

        OnArrowUp = (): void =>
        {
            var selectedItem = this.getSelectedItem();

            if (0 < selectedItem.length)
            {
                var prevItem = selectedItem.prev();

                if (0 < prevItem.length)
                {
                    selectedItem.removeClass("c-page-city-select-item-selected");
                    prevItem.addClass("c-page-city-select-item-selected");
                }
            }
        };

        OnEnter = (): void =>
        {
            var selectedItem = this.getSelectedItem();

            if (0 < selectedItem.length)
            {
                var id = parseInt(selectedItem.attr("data-id"));
                this.OnItemClicked(id);
            }
        };

        OnEscape = (): void =>
        {
            this.clearListBlock();
        };

        getSelectedItem(): JQuery
        {
            return $("div.c-page-city-select-item-selected", this.ListBlockHtml);
        }

        OnTextChange = (event: JQueryEventObject): void =>
        {
            if (0 < this.Timer)
                clearTimeout(this.Timer);

            if (this.Busy)
                return;

            this.Timer = setTimeout(this.OnTimeout, this.TimeoutMs);
        };

        OnTimeout = (): void =>
        {
            var value: string = this.Control.val().trim();

            if (value === this.SearchValue)
                return;

            this.SearchValue = value;

            if (3 > value.length)
            {
                this.clearListBlock();
                return;
            }

            //window.console.log(this.SearchValue);
            // выставляем флаг блокировки поиска и отправляем поисковый запрос на сервер
            this.Busy = true;
            this.getData(value);
        };

        getData(query: string): void
        {
            this.CurrentRequest = $.ajax({
                type: "GET",
                url: `/api/searchcity/${query}`,
                success: this.OnAjaxGetOrgDataSuccess,
                error: this.OnAjaxGetOrgDataError
            });
        }

        OnAjaxGetOrgDataError = (jqXhr: JQueryXHR, status: string, message: string): void =>
        {
            this.Busy = false;
            this.CurrentRequest = null;
            //window.console.log("_onAjaxError");
        };

        OnAjaxGetOrgDataSuccess = (data: ServerData.AjaxCityInfo[], status: string, jqXhr: JQueryXHR): void =>
        {
            this.Busy = false;
            this.CurrentRequest = null;
            //window.console.log("_onAjaxGetAccountDataSuccess");

            this.ListData = data;
            // данные получены рисуем выпадающий список выбора
            this.drawList();
        };

        drawList(): void
        {
            this.clearListBlock();
            var data = this.ListData;

            // если контрол уже потерял фокус, то ничего не отображаем
            if (false === this.Control.is(":focus"))
                return;

            if (null == data || 1 > data.length)
            {
                // сообщаем, что не найдено ни одного города
                this.Component.onCityEmpty();
                return;
            }

            this.ListBlockHtml = $('<div class="c-page-city-select-block"></div>');

            for (var i = 0; i < data.length; i++)
            {
                var city = data[i];
                var item = $('<div class="c-page-city-select-item"></div>');
                item.text(city.Fullname);
                item.attr("data-id", city.Id);
                item.bind("click", this.OnItemClick);
                item.bind("mouseenter", this.OnMouseEnter);
                item.bind("mouseleave", this.OnMouseLeave);
                this.ListBlockHtml.append(item);
            }

            // вычисляем положение списка на экране
            var top = this.Control.offset().top;
            top += this.Control.height();
            var left = this.Control.offset().left;

            this.ListBlockHtml.appendTo($("body"));
            this.ListBlockHtml.css({ top: top, left: left });
        }

        OnItemClick = (event: JQueryEventObject): void =>
        {
            this.SearchValue = null;
            var elem = $(event.delegateTarget);
            //window.console.log("this.OnItemClick(" + elem.attr("data-id") + ")");
            this.OnItemClicked(parseInt(elem.attr("data-id")));
        };

        OnItemClicked = (id: number): void =>
        {
            var city = this.getCityById(id);
            this.Component.onCitySelected(city);
            this.clearListBlock();
        };

        getCityById(id: number): ServerData.AjaxCityInfo
        {
            var city: ServerData.AjaxCityInfo = null;

            if (null != this.ListData)
            {
                for (var i = 0; i < this.ListData.length; i++)
                {
                    var c = this.ListData[i];

                    if (id === c.Id)
                    {
                        city = c;
                        break;
                    }
                }
            }

            return city;
        }

        OnLostFocus = (event: JQueryEventObject): void =>
        {
            if (null != this.ListBlockHtml)
            {
                if ($(this.ListBlockHtml).is(":hover"))
                    setTimeout(this.clearListBlock, 500);
                else
                    this.clearListBlock();
            }

            this.SearchValue = null;
            this.Component.onCitySelectedAbort();
        };
    }
}