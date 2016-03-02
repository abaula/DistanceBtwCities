using System.Collections.Generic;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Dal.Contract
{
    public interface IDbProcedures
    {
        IList<CityInfo> SearchCity(string query);
        RoutesInfoPackage SearchRouteForCity(long cityId, int maxDistance, int offset, int rows);
        RoutesInfoPackage SearchRouteForQuery(string query, int maxDistance, int offset, int rows);
        int UpdateRouteDistance(long routeId, int distance);
    }
}