///<reference path="typings\jquery\jquery.d.ts"/>
///<reference path="ServerAjaxData.ts"/>
var CitySelector;
(function (_CitySelector) {
    var CitySelector = (function () {
        function CitySelector() {
            this.control = null;
            this.component = null;
            this.timer = 0;
            this.searchValue = null;
            this.busy = false;
            this.timeoutMs = 1000;
            this.currentRequest = null;
            this.listBlockHtml = null;
            this.listData = null;
        }
        CitySelector.prototype.init = function (control, component) {
            _CitySelector.__currentCitySelector.clear();
            _CitySelector.__currentCitySelector.component = component;
            _CitySelector.__currentCitySelector.control = control;
            _CitySelector.__currentCitySelector.control.bind("cut paste", _CitySelector.__currentCitySelector.onTextChange);
            _CitySelector.__currentCitySelector.control.bind("keydown", _CitySelector.__currentCitySelector.onKeyDown);
            _CitySelector.__currentCitySelector.control.bind("blur", _CitySelector.__currentCitySelector.onLostFocus);
        };
        CitySelector.prototype.clear = function () {
            _CitySelector.__currentCitySelector.clearListBlock();
            if (0 < _CitySelector.__currentCitySelector.timer)
                clearTimeout(_CitySelector.__currentCitySelector.timer);
            if (null != _CitySelector.__currentCitySelector.control) {
                _CitySelector.__currentCitySelector.control.unbind("cut paste", _CitySelector.__currentCitySelector.onTextChange);
                _CitySelector.__currentCitySelector.control.unbind("keydown", _CitySelector.__currentCitySelector.onKeyDown);
                _CitySelector.__currentCitySelector.control.unbind("blur", _CitySelector.__currentCitySelector.onLostFocus);
            }
            // сбрасываем отправленные на сервер запросы
            if (null != _CitySelector.__currentCitySelector.currentRequest)
                _CitySelector.__currentCitySelector.currentRequest.abort();
            _CitySelector.__currentCitySelector.currentRequest = null;
            _CitySelector.__currentCitySelector.busy = false;
            _CitySelector.__currentCitySelector.listData = null;
            _CitySelector.__currentCitySelector.component = null;
        };
        CitySelector.prototype.clearListBlock = function () {
            if (null != _CitySelector.__currentCitySelector.listBlockHtml) {
                $("div", _CitySelector.__currentCitySelector.listBlockHtml).unbind("click mouseenter mouseleave");
                _CitySelector.__currentCitySelector.listBlockHtml.remove();
                _CitySelector.__currentCitySelector.listBlockHtml = null;
            }
        };
        CitySelector.prototype.onMouseEnter = function (event) {
            $("div.c-page-city-select-item", _CitySelector.__currentCitySelector.listBlockHtml).removeClass("c-page-city-select-item-selected");
            $(event.delegateTarget).addClass("c-page-city-select-item-selected");
        };
        CitySelector.prototype.onMouseLeave = function (event) {
            $("div.c-page-city-select-item", _CitySelector.__currentCitySelector.listBlockHtml).removeClass("c-page-city-select-item-selected");
        };
        CitySelector.prototype.onKeyDown = function (event) {
            if (null != _CitySelector.__currentCitySelector.listBlockHtml) {
                if (40 == event.keyCode) {
                    _CitySelector.__currentCitySelector.onArrowDown();
                    return;
                }
                else if (38 == event.keyCode) {
                    _CitySelector.__currentCitySelector.onArrowUp();
                    return;
                }
                else if (13 == event.keyCode) {
                    _CitySelector.__currentCitySelector.onEnter();
                    return;
                }
                else if (27 == event.keyCode) {
                    _CitySelector.__currentCitySelector.onEscape();
                    return;
                }
            }
            _CitySelector.__currentCitySelector.onTextChange(event);
        };
        CitySelector.prototype.onArrowDown = function () {
            var selectedItem = _CitySelector.__currentCitySelector.getSelectedItem();
            if (0 < selectedItem.length) {
                var nextItem = selectedItem.next();
                if (0 < nextItem.length) {
                    selectedItem.removeClass("c-page-city-select-item-selected");
                    nextItem.addClass("c-page-city-select-item-selected");
                }
            }
            else {
                $("div.c-page-city-select-item", _CitySelector.__currentCitySelector.listBlockHtml).first().addClass("c-page-city-select-item-selected");
            }
        };
        CitySelector.prototype.onArrowUp = function () {
            var selectedItem = _CitySelector.__currentCitySelector.getSelectedItem();
            if (0 < selectedItem.length) {
                var prevItem = selectedItem.prev();
                if (0 < prevItem.length) {
                    selectedItem.removeClass("c-page-city-select-item-selected");
                    prevItem.addClass("c-page-city-select-item-selected");
                }
            }
        };
        CitySelector.prototype.onEnter = function () {
            var selectedItem = _CitySelector.__currentCitySelector.getSelectedItem();
            if (0 < selectedItem.length) {
                var id = parseInt(selectedItem.attr("data-id"));
                _CitySelector.__currentCitySelector.onItemClicked(id);
            }
        };
        CitySelector.prototype.onEscape = function () {
            _CitySelector.__currentCitySelector.clearListBlock();
        };
        CitySelector.prototype.getSelectedItem = function () {
            return $("div.c-page-city-select-item-selected", _CitySelector.__currentCitySelector.listBlockHtml);
        };
        CitySelector.prototype.onTextChange = function (event) {
            if (0 < _CitySelector.__currentCitySelector.timer)
                clearTimeout(_CitySelector.__currentCitySelector.timer);
            if (true == _CitySelector.__currentCitySelector.busy)
                return;
            _CitySelector.__currentCitySelector.timer = setTimeout(_CitySelector.__currentCitySelector.onTimeout, _CitySelector.__currentCitySelector.timeoutMs);
        };
        CitySelector.prototype.onTimeout = function () {
            var value = _CitySelector.__currentCitySelector.control.val().trim();
            if (value == _CitySelector.__currentCitySelector.searchValue)
                return;
            _CitySelector.__currentCitySelector.searchValue = value;
            if (3 > value.length) {
                _CitySelector.__currentCitySelector.clearListBlock();
                return;
            }
            //window.console.log(__currentCitySelector.searchValue);
            // выставляем флаг блокировки поиска и отправляем поисковый запрос на сервер
            _CitySelector.__currentCitySelector.busy = true;
            _CitySelector.__currentCitySelector.getData(value);
        };
        CitySelector.prototype.getData = function (query) {
            _CitySelector.__currentCitySelector.currentRequest = $.ajax({
                type: "GET",
                url: "/api/searchcity/" + query,
                success: _CitySelector.__currentCitySelector.onAjaxGetOrgDataSuccess,
                error: _CitySelector.__currentCitySelector.onAjaxGetOrgDataError
            });
        };
        CitySelector.prototype.onAjaxGetOrgDataError = function (jqXHR, status, message) {
            _CitySelector.__currentCitySelector.busy = false;
            _CitySelector.__currentCitySelector.currentRequest = null;
            //window.console.log("_onAjaxError");
        };
        CitySelector.prototype.onAjaxGetOrgDataSuccess = function (data, status, jqXHR) {
            _CitySelector.__currentCitySelector.busy = false;
            _CitySelector.__currentCitySelector.currentRequest = null;
            //window.console.log("_onAjaxGetAccountDataSuccess");
            _CitySelector.__currentCitySelector.listData = data;
            // данные получены рисуем выпадающий список выбора
            _CitySelector.__currentCitySelector.drawList();
        };
        CitySelector.prototype.drawList = function () {
            _CitySelector.__currentCitySelector.clearListBlock();
            var data = _CitySelector.__currentCitySelector.listData;
            // если контрол уже потерял фокус, то ничего не отображаем
            if (false == _CitySelector.__currentCitySelector.control.is(":focus"))
                return;
            if (null == data || 1 > data.length) {
                // сообщаем, что не найдено ни одного города
                _CitySelector.__currentCitySelector.component.onCityEmpty();
                return;
            }
            _CitySelector.__currentCitySelector.listBlockHtml = $('<div class="c-page-city-select-block"></div>');
            for (var i = 0; i < data.length; i++) {
                var city = data[i];
                var item = $('<div class="c-page-city-select-item"></div>');
                item.text(city.Fullname);
                item.attr("data-id", city.Id);
                item.bind("click", _CitySelector.__currentCitySelector.onItemClick);
                item.bind("mouseenter", _CitySelector.__currentCitySelector.onMouseEnter);
                item.bind("mouseleave", _CitySelector.__currentCitySelector.onMouseLeave);
                _CitySelector.__currentCitySelector.listBlockHtml.append(item);
            }
            // вычисляем положение списка на экране
            var top = _CitySelector.__currentCitySelector.control.offset().top;
            top += _CitySelector.__currentCitySelector.control.height();
            var left = _CitySelector.__currentCitySelector.control.offset().left;
            _CitySelector.__currentCitySelector.listBlockHtml.appendTo($("body"));
            _CitySelector.__currentCitySelector.listBlockHtml.css({ top: top, left: left });
        };
        CitySelector.prototype.onItemClick = function (event) {
            _CitySelector.__currentCitySelector.searchValue = null;
            var elem = $(event.delegateTarget);
            //window.console.log("__currentCitySelector.onItemClick(" + elem.attr("data-id") + ")");
            _CitySelector.__currentCitySelector.onItemClicked(parseInt(elem.attr("data-id")));
        };
        CitySelector.prototype.onItemClicked = function (id) {
            var city = _CitySelector.__currentCitySelector.getCityById(id);
            _CitySelector.__currentCitySelector.component.onCitySelected(city);
            _CitySelector.__currentCitySelector.clearListBlock();
        };
        CitySelector.prototype.getCityById = function (id) {
            var city = null;
            if (null != _CitySelector.__currentCitySelector.listData) {
                for (var i = 0; i < _CitySelector.__currentCitySelector.listData.length; i++) {
                    var c = _CitySelector.__currentCitySelector.listData[i];
                    if (id == c.Id) {
                        city = c;
                        break;
                    }
                }
            }
            return city;
        };
        CitySelector.prototype.onLostFocus = function (event) {
            if (null != _CitySelector.__currentCitySelector.listBlockHtml) {
                if ($(_CitySelector.__currentCitySelector.listBlockHtml).is(':hover'))
                    setTimeout(_CitySelector.__currentCitySelector.clearListBlock, 500);
                else
                    _CitySelector.__currentCitySelector.clearListBlock();
            }
            _CitySelector.__currentCitySelector.searchValue = null;
            _CitySelector.__currentCitySelector.component.onCitySelectedAbort();
        };
        return CitySelector;
    })();
    _CitySelector.CitySelector = CitySelector;
    _CitySelector.__currentCitySelector = new CitySelector();
})(CitySelector || (CitySelector = {}));
//# sourceMappingURL=CitySelector.js.map