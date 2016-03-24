using DistanceBtwCities.Dal.Contract;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    /// <summary>
    /// Сервис изменения данных маршрута.
    /// </summary>
    public class RouteEditService : IRouteEditService
    {
        private readonly IDbProcedures _dbProcedures;

        public RouteEditService(IDbProcedures dbProcedures)
        {
            _dbProcedures = dbProcedures;
        }

        /// <summary>
        /// Изменение значения дистанции маршрута.
        /// </summary>
        /// <param name="routeInfo">Данные маршрута.</param>
        /// <returns>Новый экземпляр маршрута с обновлёнными значениями.</returns>
        public RouteInfo UpdateRouteDistance(RouteInfo routeInfo)
        {
            _dbProcedures.UpdateRouteDistance(routeInfo.Id, routeInfo.Distance);

            // Возвращаем новый экземпляр объекта
            return new RouteInfo {Id = routeInfo.Id, Distance = routeInfo.Distance};
        }
    }
}