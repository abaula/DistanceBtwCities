using System.Threading.Tasks;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    public interface IRouteEditService
    {
        Task<RouteInfo> UpdateRouteDistance(RouteInfo routeInfo);
    }
}