using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistanceBtwCities.DatabaseApi;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    class SearchCityTask : ISearchCityTask
    {
        private string _connectionString;

        public List<CityInfo> Cities { get; private set; }

        public SearchCityTask(string connectionString)
        {
            _connectionString = connectionString;
            Cities = new List<CityInfo>();
        }

        public Task SearchCityAsync(string query)
        {
            return Task.Run(() =>
            {
                _searchCity(query);
            });
        }

        private void _searchCity(string query)
        {
            var dbProcedures = DatabaseApiFactory.CreateDbProcedures(_connectionString);
            var cities = dbProcedures.SearchCity(query);
            Cities.AddRange(cities);
        }

    }
}
