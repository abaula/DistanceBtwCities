using System.Threading.Tasks;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    public interface ISearchRouteService
    {
        Task<RoutesInfoPackage> SearchRouteForQuery(string query, int maxDistance, int offset, int rows);
        Task<RoutesInfoPackage> SearchRouteForCity(long cityId, int maxDistance, int offset, int rows);
    }
}