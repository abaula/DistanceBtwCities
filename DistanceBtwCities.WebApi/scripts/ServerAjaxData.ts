

module ServerData
{
    export class AjaxCityInfo
    {
        Id: number;
        Latitude: number;
        Longitude: number;
        Name: string;
        District: string;
        Region: string;
        Suffix: string;
        CladrCode: string;
        PostCode: string;
        Fullname: string;
    } 

    export class AjaxRouteInfo
    {
        Id: number;
        City1: AjaxCityInfo;
        City2: AjaxCityInfo;
        Distance: number;
    }

    export class AjaxRoutesInfoPackage
    {
        Routes: AjaxRouteInfo[];
        AllFoundRoutesCount: number;
    }
}