using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DistanceBtwCities.DataContract
{
    /// <summary>
    /// Пакет данных с результатом поиска - содержит данные одной страницы указанного в запросе размера.
    /// </summary>
    [DataContract]
    public class RoutesInfoPackage
    {
        public RoutesInfoPackage()
        {
            Routes = new List<RouteInfo>();
        }

        /// <summary>
        /// Список записей маршрутов.
        /// </summary>
        [DataMember]
        public List<RouteInfo> Routes { get; }

        /// <summary>
        /// Общее количество найденных маршрутов в базе.
        /// </summary>
        [DataMember]
        public int AllFoundRoutesCount { get; set; }
    }
}