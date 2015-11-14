///<reference path="typings\jquery\jquery.d.ts"/>
///<reference path="ServerAjaxData.ts"/>
///<reference path="CitySelector.ts"/>

module DistanceEditor
{
    export class DistanceEditorController implements CitySelector.ICitySelector
    {
        selectedSearchCity: ServerData.AjaxCityInfo;

        onSearchButtonClick(event: JQueryEventObject): void 
        {
        }

        applySearchCityData(): void 
        {
            var city: ServerData.AjaxCityInfo = __currDistanceEditor.selectedSearchCity;
            var fullName: string = "";

            if (null != city)
                fullName = city.Fullname;

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

            //event.delegateTarget.
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
        }
    }  
    
    export var __currDistanceEditor: DistanceEditorController = new DistanceEditorController();       
}

// настраиваем обработчик события о готовности страницы
$(document).ready(DistanceEditor.__currDistanceEditor.onDocumentReady);
