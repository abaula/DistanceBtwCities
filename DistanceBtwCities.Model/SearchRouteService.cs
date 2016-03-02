using System.Threading.Tasks;
using DistanceBtwCities.Dal.Contract;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    public class SearchRouteService : ISearchRouteService
    {
        private readonly IDbProcedures _dbProcedures;

        public SearchRouteService(IDbProcedures dbProcedures)
        {
            _dbProcedures = dbProcedures;
        }

        public Task<RoutesInfoPackage> SearchRouteForQuery(string query, int maxDistance, int offset, int rows)
        {
            return Task.Run(() => _searchRouteForQuery(query, maxDistance, offset, rows));
        }

        public Task<RoutesInfoPackage> SearchRouteForCity(long cityId, int maxDistance, int offset, int rows)
        {
            return Task.Run(() => _searchRouteForCity(cityId, maxDistance, offset, rows));
        }

        private RoutesInfoPackage _searchRouteForQuery(string query, int maxDistance, int offset, int rows)
        {
            var package = _dbProcedures.SearchRouteForQuery(query, maxDistance, offset, rows);
            return package;
        }

        private RoutesInfoPackage _searchRouteForCity(long cityId, int maxDistance, int offset, int rows)
        {
            var package = _dbProcedures.SearchRouteForCity(cityId, maxDistance, offset, rows);
            return package;
        }
    }
}