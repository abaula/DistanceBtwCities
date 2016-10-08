
namespace DistanceBtwCities.Common.Dtos
{
    /// <summary>
    /// Информация о маршруте.
    /// <remarks>
    /// Содержит информацию о маршруте между двумя городами. 
    /// Дистанция задаётся в километрах.
    /// Нумерация городов 1 и 2 чисто условная - дистанция между двумя городами одна и та же вне зависимости от направления маршрута.
    /// </remarks>
    /// </summary>
    public class RouteInfo
    {
        /// <summary>
        /// Идентификатор маршрута.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Данные о городе маршрута.
        /// </summary>
        public CityInfo City1 { get; set; }

        /// <summary>
        /// Данные о городе маршрута.
        /// </summary>
        public CityInfo City2 { get; set; }

        /// <summary>
        /// Дистанция между городами в км.
        /// </summary>
        public int Distance { get; set; }
    }
}