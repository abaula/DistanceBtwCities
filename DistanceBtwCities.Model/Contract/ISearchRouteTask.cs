using System.Threading.Tasks;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    public interface ISearchRouteTask
    {
        RoutesInfoPackage Package { get; set; }
        Task SearchRouteForQuery(string query, int maxDistance, int offset, int rows);
        Task SearchRouteForCity(long cityId, int maxDistance, int offset, int rows);
    }
}