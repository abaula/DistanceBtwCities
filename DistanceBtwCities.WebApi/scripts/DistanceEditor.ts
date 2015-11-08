///<reference path="typings\jquery\jquery.d.ts"/>
///<reference path="ServerAjaxData.ts"/>
///<reference path="CitySelector.ts"/>

module DistanceEditor
{
    export class DistanceEditorController implements CitySelector.ICitySelector
    {
        citySearchTxtBound: boolean = false;
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

            $("#i-page-search-form-city-txt").val(fullName);
        }

        clearSearchCityData(): void 
        {
            $("#i-page-search-form-city-txt").val('');
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


            if (__currDistanceEditor.citySearchTxtBound)
                return;

            __currDistanceEditor.citySearchTxtBound = true;

            // подключаем контрол выбора города
            CitySelector.__currentCitySelector.init($("#i-page-search-form-city-txt"), __currDistanceEditor);
        }

        onDocumentReady(): void
        {
            /////////////////////////////////////
            // цепляем обработчики событий
            $("#i-page-search-form-city-txt").focus(__currDistanceEditor.onCitySearchTxtFocus);

            // навигация по разделам профайла
            //$("#i-ctrl-profile-navigation-block > div").click(__currentComp.onProfileNavigationMenuItemClick);
            //$("#i-ctrl-profile-update-btn").click(__currentComp.onUpdateButtonClick);
        }
    }  
    
    export var __currDistanceEditor: DistanceEditorController = new DistanceEditorController();       
}

// настраиваем обработчик события о готовности страницы
$(document).ready(DistanceEditor.__currDistanceEditor.onDocumentReady);
