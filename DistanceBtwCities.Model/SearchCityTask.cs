using System.Collections.Generic;
using System.Threading.Tasks;
using DistanceBtwCities.DatabaseApi;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    internal class SearchCityTask : ISearchCityTask
    {
        private readonly string _connectionString;

        public SearchCityTask(string connectionString)
        {
            _connectionString = connectionString;
            Cities = new List<CityInfo>();
        }

        public List<CityInfo> Cities { get; }

        public Task SearchCityAsync(string query)
        {
            return Task.Run(() => { _searchCity(query); });
        }

        private void _searchCity(string query)
        {
            var dbProcedures = DatabaseApiFactory.CreateDbProcedures(_connectionString);
            var cities = dbProcedures.SearchCity(query);
            Cities.AddRange(cities);
        }
    }
}