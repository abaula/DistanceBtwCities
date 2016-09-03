using System.Collections.Generic;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    /// <summary>
    /// Интерфейс сервиса поиска городов.
    /// </summary>
    public interface ISearchCityService
    {
        /// <summary>
        /// Поиск города по запросу - первым символам имени города.
        /// <remarks>Поиск выполняется по указанной подстроке в имени города, начиная с начала города. 
        /// Вот так работает поиск - запрос: "Кра", ответ: "Красавино", "Красновишерск"...</remarks>
        /// </summary>
        /// <param name="query">Поисковый запрос.</param>
        /// <returns>Список найденных городов.</returns>
        IList<CityInfo> SearchCityAsync(string query);
    }
}