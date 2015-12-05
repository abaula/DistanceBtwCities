using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistanceBtwCities.DatabaseApi;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    class RouteEditTask : IRouteEditTask
    {
        private string _connectionString;

        public RouteEditTask(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<RouteInfo> UpdateRouteDistance(RouteInfo routeInfo)
        {
            return Task.Run(() =>
            {
                var dbProcedures = DatabaseApiFactory.CreateDbProcedures(_connectionString);
                dbProcedures.UpdateRouteDistance(routeInfo.Id, routeInfo.Distance);

                // Возвращаем новый экземпляр объекта
                return new RouteInfo { Id = routeInfo.Id, Distance = routeInfo.Distance };
            });
        }
    }
}
