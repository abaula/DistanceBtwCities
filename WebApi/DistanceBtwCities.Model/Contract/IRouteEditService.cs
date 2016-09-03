using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    /// <summary>
    /// Интерфейс сервиса изменения данных маршрута.
    /// </summary>
    public interface IRouteEditService
    {
        /// <summary>
        /// Изменение значения дистанции маршрута.
        /// </summary>
        /// <param name="routeInfo">Данные маршрута.</param>
        /// <returns>Новый экземпляр маршрута с обновлёнными значениями.</returns>
        RouteInfo UpdateRouteDistance(RouteInfo routeInfo);
    }
}