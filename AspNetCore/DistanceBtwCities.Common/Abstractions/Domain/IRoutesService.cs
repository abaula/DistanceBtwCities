using System.Threading.Tasks;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Common.Dtos.Requests;

namespace DistanceBtwCities.Common.Abstractions.Domain
{
    /// <summary>
    /// Интерфейс сервиса доступа к данным маршрута.
    /// </summary>
    public interface IRoutesService
    {
        /// <summary>
        /// Поиск маршрута по поисковому запросу.
        /// </summary>
        Task<RoutesInfoPackage> SearchRoute(RouteSearchRequestDto request);

        /// <summary>
        /// Изменение значения дистанции маршрута.
        /// </summary>
        Task UpdateRouteDistance(RouteUpdateInfo request);
    }
}