/// <reference path="typings\jquery\jquery.d.ts" />
/// <reference path="ServerAjaxData.ts" />
var CitySelector;
(function (CitySelector_1) {
    var CitySelector = (function () {
        function CitySelector(control, component) {
            var _this = this;
            this.Control = null;
            this.Component = null;
            this.Timer = 0;
            this.SearchValue = null;
            this.Busy = false;
            this.TimeoutMs = 1000;
            this.CurrentRequest = null;
            this.ListBlockHtml = null;
            this.ListData = null;
            this.OnMouseEnter = function (event) {
                $("div.c-page-city-select-item", _this.ListBlockHtml).removeClass("c-page-city-select-item-selected");
                $(event.delegateTarget).addClass("c-page-city-select-item-selected");
            };
            this.OnMouseLeave = function (event) {
                $("div.c-page-city-select-item", _this.ListBlockHtml).removeClass("c-page-city-select-item-selected");
            };
            this.OnKeyDown = function (event) {
                if (null != _this.ListBlockHtml) {
                    if (40 === event.keyCode) 
                    // arrow down
                    {
                        _this.OnArrowDown();
                        return;
                    }
                    else if (38 === event.keyCode) 
                    // arrow up
                    {
                        _this.OnArrowUp();
                        return;
                    }
                    else if (13 === event.keyCode) 
                    // enter
                    {
                        _this.OnEnter();
                        return;
                    }
                    else if (27 === event.keyCode) 
                    // escape
                    {
                        _this.OnEscape();
                        return;
                    }
                }
                _this.OnTextChange(event);
            };
            this.OnArrowDown = function () {
                var selectedItem = _this.getSelectedItem();
                if (0 < selectedItem.length) {
                    var nextItem = selectedItem.next();
                    if (0 < nextItem.length) {
                        selectedItem.removeClass("c-page-city-select-item-selected");
                        nextItem.addClass("c-page-city-select-item-selected");
                    }
                }
                else {
                    $("div.c-page-city-select-item", _this.ListBlockHtml).first().addClass("c-page-city-select-item-selected");
                }
            };
            this.OnArrowUp = function () {
                var selectedItem = _this.getSelectedItem();
                if (0 < selectedItem.length) {
                    var prevItem = selectedItem.prev();
                    if (0 < prevItem.length) {
                        selectedItem.removeClass("c-page-city-select-item-selected");
                        prevItem.addClass("c-page-city-select-item-selected");
                    }
                }
            };
            this.OnEnter = function () {
                var selectedItem = _this.getSelectedItem();
                if (0 < selectedItem.length) {
                    var id = parseInt(selectedItem.attr("data-id"));
                    _this.OnItemClicked(id);
                }
            };
            this.OnEscape = function () {
                _this.clearListBlock();
            };
            this.OnTextChange = function (event) {
                if (0 < _this.Timer)
                    clearTimeout(_this.Timer);
                if (_this.Busy)
                    return;
                _this.Timer = setTimeout(_this.OnTimeout, _this.TimeoutMs);
            };
            this.OnTimeout = function () {
                var value = _this.Control.val().trim();
                if (value === _this.SearchValue)
                    return;
                _this.SearchValue = value;
                if (3 > value.length) {
                    _this.clearListBlock();
                    return;
                }
                //window.console.log(this.SearchValue);
                // выставляем флаг блокировки поиска и отправляем поисковый запрос на сервер
                _this.Busy = true;
                _this.getData(value);
            };
            this.OnAjaxGetOrgDataError = function (jqXhr, status, message) {
                _this.Busy = false;
                _this.CurrentRequest = null;
                //window.console.log("_onAjaxError");
            };
            this.OnAjaxGetOrgDataSuccess = function (data, status, jqXhr) {
                _this.Busy = false;
                _this.CurrentRequest = null;
                //window.console.log("_onAjaxGetAccountDataSuccess");
                _this.ListData = data;
                // данные получены рисуем выпадающий список выбора
                _this.drawList();
            };
            this.OnItemClick = function (event) {
                _this.SearchValue = null;
                var elem = $(event.delegateTarget);
                //window.console.log("this.OnItemClick(" + elem.attr("data-id") + ")");
                _this.OnItemClicked(parseInt(elem.attr("data-id")));
            };
            this.OnItemClicked = function (id) {
                var city = _this.getCityById(id);
                _this.Component.onCitySelected(city);
                _this.clearListBlock();
            };
            this.OnLostFocus = function (event) {
                if (null != _this.ListBlockHtml) {
                    if ($(_this.ListBlockHtml).is(":hover"))
                        setTimeout(_this.clearListBlock, 500);
                    else
                        _this.clearListBlock();
                }
                _this.SearchValue = null;
                _this.Component.onCitySelectedAbort();
            };
            this.Component = component;
            this.Control = control;
            this.Control.bind("cut paste", this.OnTextChange);
            this.Control.bind("keydown", this.OnKeyDown);
            this.Control.bind("blur", this.OnLostFocus);
        }
        CitySelector.prototype.clearListBlock = function () {
            if (null != this.ListBlockHtml) {
                $("div", this.ListBlockHtml).unbind("click mouseenter mouseleave");
                this.ListBlockHtml.remove();
                this.ListBlockHtml = null;
            }
        };
        CitySelector.prototype.getSelectedItem = function () {
            return $("div.c-page-city-select-item-selected", this.ListBlockHtml);
        };
        CitySelector.prototype.getData = function (query) {
            this.CurrentRequest = $.ajax({
                type: "GET",
                url: "/api/searchcity/" + query,
                success: this.OnAjaxGetOrgDataSuccess,
                error: this.OnAjaxGetOrgDataError
            });
        };
        CitySelector.prototype.drawList = function () {
            this.clearListBlock();
            var data = this.ListData;
            // если контрол уже потерял фокус, то ничего не отображаем
            if (false === this.Control.is(":focus"))
                return;
            if (null == data || 1 > data.length) {
                // сообщаем, что не найдено ни одного города
                this.Component.onCityEmpty();
                return;
            }
            this.ListBlockHtml = $('<div class="c-page-city-select-block"></div>');
            for (var i = 0; i < data.length; i++) {
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
        };
        CitySelector.prototype.getCityById = function (id) {
            var city = null;
            if (null != this.ListData) {
                for (var i = 0; i < this.ListData.length; i++) {
                    var c = this.ListData[i];
                    if (id === c.Id) {
                        city = c;
                        break;
                    }
                }
            }
            return city;
        };
        return CitySelector;
    })();
    CitySelector_1.CitySelector = CitySelector;
})(CitySelector || (CitySelector = {}));
