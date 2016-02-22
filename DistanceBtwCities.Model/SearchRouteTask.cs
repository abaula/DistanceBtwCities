using System.Threading.Tasks;
using DistanceBtwCities.DatabaseApi;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    internal class SearchRouteTask : ISearchRouteTask
    {
        private readonly string _connectionString;

        public SearchRouteTask(string connectionString)
        {
            _connectionString = connectionString;
        }

        public RoutesInfoPackage Package { get; set; }

        public Task SearchRouteForQuery(string query, int maxDistance, int offset, int rows)
        {
            return Task.Run(() => { _searchRouteForQuery(query, maxDistance, offset, rows); });
        }

        public Task SearchRouteForCity(long cityId, int maxDistance, int offset, int rows)
        {
            return Task.Run(() => { _searchRouteForCity(cityId, maxDistance, offset, rows); });
        }

        private void _searchRouteForQuery(string query, int maxDistance, int offset, int rows)
        {
            var dbProcedures = DatabaseApiFactory.CreateDbProcedures(_connectionString);
            var package = dbProcedures.SearchRouteForQuery(query, maxDistance, offset, rows);
            Package = package;
        }

        private void _searchRouteForCity(long cityId, int maxDistance, int offset, int rows)
        {
            var dbProcedures = DatabaseApiFactory.CreateDbProcedures(_connectionString);
            var package = dbProcedures.SearchRouteForCity(cityId, maxDistance, offset, rows);
            Package = package;
        }
    }
}