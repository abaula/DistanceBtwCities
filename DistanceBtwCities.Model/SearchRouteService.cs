using DistanceBtwCities.Dal.Contract;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Interception;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    /// <summary>
    /// Сервис для поиска маршрутов.
    /// </summary>
    public class SearchRouteService : ISearchRouteService
    {
        private readonly IDbProcedures _dbProcedures;

        public SearchRouteService(IDbProcedures dbProcedures)
        {
            _dbProcedures = dbProcedures;
        }

        /// <summary>
        /// Поиск маршрута по поисковому запросу.
        /// </summary>
        /// <param name="query">Поисковый запрос - первые символы названия города.</param>
        /// <param name="maxDistance">Ограничение по максимальной дистанции маршрута - в результат попадают маршруты у которых дистанция меньше или равна указанной.</param>
        /// <param name="offset">Номер первой записи из найденнго списка, которая попадает в возвращаемый результат.</param>
        /// <param name="rows">Количество возвращаемых записей.</param>
        /// <returns>Выборка из найденных маршрутов в соответствии с параметрами <paramref name="offset"/> и <paramref name="rows"/>.</returns>
        [Transaction]
        public RoutesInfoPackage SearchRouteForQuery(string query, int maxDistance, int offset, int rows)
        {
            var package = _dbProcedures.SearchRouteForQuery(query, maxDistance, offset, rows);
            return package;
        }

        /// <summary>
        /// Поиск маршрута по идентификатору города.
        /// </summary>
        /// <param name="cityId">Идентификатор города.</param>
        /// <param name="maxDistance">Ограничение по максимальной дистанции маршрута - в результат попадают маршруты у которых дистанция меньше или равна указанной.</param>
        /// <param name="offset">Номер первой записи из найденнго списка, которая попадает в возвращаемый результат.</param>
        /// <param name="rows">Количество возвращаемых записей.</param>
        /// <returns>Выборка из найденных маршрутов в соответствии с параметрами <paramref name="offset"/> и <paramref name="rows"/>.</returns>
        [Transaction]
        public RoutesInfoPackage SearchRouteForCity(long cityId, int maxDistance, int offset, int rows)
        {
            var package = _dbProcedures.SearchRouteForCity(cityId, maxDistance, offset, rows);
            return package;
        }
    }
}