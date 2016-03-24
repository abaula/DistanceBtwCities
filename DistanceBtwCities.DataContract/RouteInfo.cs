using System.Runtime.Serialization;

namespace DistanceBtwCities.DataContract
{
    /// <summary>
    /// Информация о маршруте.
    /// <remarks>
    /// Содержит информацию о маршруте между двумя городами. 
    /// Дистанция задаётся в километрах.
    /// Нумерация городов 1 и 2 чисто условная - дистанция между двумя городами одна и та же вне зависимости от направления маршрута.
    /// </remarks>
    /// </summary>
    [DataContract]
    public class RouteInfo
    {
        /// <summary>
        /// Идентификатор маршрута.
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// Данные о городе маршрута.
        /// </summary>
        [DataMember]
        public CityInfo City1 { get; set; }

        /// <summary>
        /// Данные о городе маршрута.
        /// </summary>
        [DataMember]
        public CityInfo City2 { get; set; }

        /// <summary>
        /// Дистанция между городами в км.
        /// </summary>
        [DataMember]
        public int Distance { get; set; }
    }
}