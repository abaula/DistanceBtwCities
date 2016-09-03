using System.Collections.Generic;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Dal.Contract
{
    public interface IDbProcedures
    {
        /// <summary>
        /// Поиск городов по соответствию поисковому запросу.
        /// <remarks>Соответствие ищется с начала имени города.</remarks>
        /// </summary>
        /// <param name="query">Поисковый запрос - начало имени города.</param>
        /// <returns>Список найденных городов.</returns>
        IList<CityInfo> SearchCity(string query);

        /// <summary>
        /// Поиск маршрутов для указанного города.
        /// <remarks>Поиск ограничен параметром <paramref name="maxDistance"/>.</remarks>
        /// </summary>
        /// <param name="cityId">Id города.</param>
        /// <param name="maxDistance">Максимальная дистанция маршрута - маршруты с большей дистанцией не включаются в результат поиска.</param>
        /// <param name="offset">Номер первого возвращаего маршрута из найденных.</param>
        /// <param name="rows">Количество возвращаемых найденных маршрутов.</param>
        /// <returns>Список найденных маршрутов, начиная с маршрута на позиции указанной в параметре <paramref name="offset"/>. 
        /// Количесто возвращаемых записей указано в параметре <paramref name="rows"/>.</returns>
        RoutesInfoPackage SearchRouteForCity(long cityId, int maxDistance, int offset, int rows);

        /// <summary>
        /// Поиск маршрутов по соответствию поисковому запросу - имени города.
        /// <remarks>Поиск ограничен параметром <paramref name="maxDistance"/>.</remarks>
        /// </summary>
        /// <param name="query">Поисковый запрос - начало имени города.</param>
        /// <param name="maxDistance">Максимальная дистанция маршрута - маршруты с большей дистанцией не включаются в результат поиска.</param>
        /// <param name="offset">Номер первого возвращаего маршрута из найденных.</param>
        /// <param name="rows">Количество возвращаемых найденных маршрутов.</param>
        /// <returns>Список найденных маршрутов, начиная с маршрута на позиции указанной в параметре <paramref name="offset"/>. 
        /// Количесто возвращаемых записей указано в параметре <paramref name="rows"/>.</returns>
        RoutesInfoPackage SearchRouteForQuery(string query, int maxDistance, int offset, int rows);

        /// <summary>
        /// Обновления значения дистаниции в справочнике маршрутов.
        /// </summary>
        /// <param name="routeId">Id маршрута между двумя городами.</param>
        /// <param name="distance">Новое значение дистанции.</param>
        /// <returns>Количество обновлённых записей в БД.</returns>
        int UpdateRouteDistance(long routeId, int distance);
    }
}