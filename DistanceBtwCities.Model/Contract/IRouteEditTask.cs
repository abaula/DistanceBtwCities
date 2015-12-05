using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    public interface IRouteEditTask
    {
        Task<RouteInfo> UpdateRouteDistance(RouteInfo routeInfo);
    }
}
