
namespace DistanceBtwCities.Common.Dtos
{
    public class RouteUpdateInfo
    {
        /// <summary>
        /// Идентификатор маршрута.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дистанция между городами в км.
        /// </summary>
        public int Distance { get; set; }
    }
}
