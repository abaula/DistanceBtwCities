using System.Collections.Generic;
using System.Threading.Tasks;
using DistanceBtwCities.Dal.Contract;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    public class SearchCityService : ISearchCityService
    {
        private readonly IDbProcedures _dbProcedures;

        public SearchCityService(IDbProcedures dbProcedures)
        {
            _dbProcedures = dbProcedures;
        }

        public Task<IList<CityInfo>> SearchCityAsync(string query)
        {
            return Task.Run(() => _searchCity(query));
        }

        private IList<CityInfo> _searchCity(string query)
        {
            var cities = _dbProcedures.SearchCity(query);
            return cities;
        }
    }
}