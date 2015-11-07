using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    public class ModelFactory
    {
        private string _connectionString;

        private ModelFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static ModelFactory CreateInstance(string connectionString)
        {
            return new ModelFactory(connectionString);
        }

        public ISearchCityTask CreateSearchCityTask()
        {
            return new SearchCityTask(_connectionString);
        }

        public ISearchRouteTask CreateSearchRouteTask()
        {
            return new SearchRouteTask(_connectionString);
        }
    }
}
