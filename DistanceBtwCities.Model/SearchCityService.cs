using System.Collections.Generic;
using DistanceBtwCities.Dal.Contract;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    /// <summary>
    /// Сервис поиска городов.
    /// </summary>
    public class SearchCityService : ISearchCityService
    {
        private readonly IDbProcedures _dbProcedures;

        public SearchCityService(IDbProcedures dbProcedures)
        {
            _dbProcedures = dbProcedures;
        }

        /// <summary>
        /// Поиск города по запросу - первым символам имени города.
        /// <remarks>Поиск выполняется по указанной подстроке в имени города, начиная с начала города. 
        /// Вот так работает поиск - запрос: "Кра", ответ: "Красавино", "Красновишерск"...</remarks>
        /// </summary>
        /// <param name="query">Поисковый запрос.</param>
        /// <returns>Список найденных городов.</returns>
        public IList<CityInfo> SearchCityAsync(string query)
        {
            return _searchCity(query);
        }

        private IList<CityInfo> _searchCity(string query)
        {
            var cities = _dbProcedures.SearchCity(query);
            return cities;
        }
    }
}