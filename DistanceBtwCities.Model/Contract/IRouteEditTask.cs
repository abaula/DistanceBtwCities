using System.Threading.Tasks;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    public interface IRouteEditTask
    {
        Task<RouteInfo> UpdateRouteDistance(RouteInfo routeInfo);
    }
}