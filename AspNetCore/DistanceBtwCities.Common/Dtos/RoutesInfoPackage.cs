
namespace DistanceBtwCities.Common.Dtos
{
    /// <summary>
    /// Пакет данных с результатом поиска - содержит данные одной страницы указанного в запросе размера.
    /// </summary>
    public class RoutesInfoPackage
    {
        /// <summary>
        /// Список записей маршрутов.
        /// </summary>
        public RouteInfo[] Routes { get; set; }

        /// <summary>
        /// Общее количество найденных маршрутов в базе.
        /// </summary>
        public int AllFoundRoutesCount { get; set; }
    }
}