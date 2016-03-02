using System.Threading.Tasks;
using DistanceBtwCities.Dal.Contract;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    public class RouteEditService : IRouteEditService
    {
        private readonly IDbProcedures _dbProcedures;

        public RouteEditService(IDbProcedures dbProcedures)
        {
            _dbProcedures = dbProcedures;
        }

        public Task<RouteInfo> UpdateRouteDistance(RouteInfo routeInfo)
        {
            return Task.Run(() =>
            {
                _dbProcedures.UpdateRouteDistance(routeInfo.Id, routeInfo.Distance);

                // Возвращаем новый экземпляр объекта
                return new RouteInfo {Id = routeInfo.Id, Distance = routeInfo.Distance};
            });
        }
    }
}