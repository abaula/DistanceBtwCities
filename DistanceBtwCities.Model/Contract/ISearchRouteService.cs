using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    /// <summary>
    /// Интерфейс сервиса для поиска маршрутов.
    /// </summary>
    public interface ISearchRouteService
    {
        /// <summary>
        /// Поиск маршрута по поисковому запросу.
        /// </summary>
        /// <param name="query">Поисковый запрос - первые символы названия города.</param>
        /// <param name="maxDistance">Ограничение по максимальной дистанции маршрута - в результат попадают маршруты у которых дистанция меньше или равна указанной.</param>
        /// <param name="offset">Номер первой записи из найденнго списка, которая попадает в возвращаемый результат.</param>
        /// <param name="rows">Количество возвращаемых записей.</param>
        /// <returns>Выборка из найденных маршрутов в соответствии с параметрами <paramref name="offset"/> и <paramref name="rows"/>.</returns>
        RoutesInfoPackage SearchRouteForQuery(string query, int maxDistance, int offset, int rows);

        /// <summary>
        /// Поиск маршрута по идентификатору города.
        /// </summary>
        /// <param name="cityId">Идентификатор города.</param>
        /// <param name="maxDistance">Ограничение по максимальной дистанции маршрута - в результат попадают маршруты у которых дистанция меньше или равна указанной.</param>
        /// <param name="offset">Номер первой записи из найденнго списка, которая попадает в возвращаемый результат.</param>
        /// <param name="rows">Количество возвращаемых записей.</param>
        /// <returns>Выборка из найденных маршрутов в соответствии с параметрами <paramref name="offset"/> и <paramref name="rows"/>.</returns>
        RoutesInfoPackage SearchRouteForCity(long cityId, int maxDistance, int offset, int rows);
    }
}