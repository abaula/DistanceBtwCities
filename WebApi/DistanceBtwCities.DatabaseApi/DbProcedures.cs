using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DistanceBtwCities.Dal.Adapters;
using DistanceBtwCities.Dal.Contract;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Dal
{
    public class DbProcedures : DbProceduresBase, IDbProcedures
    {
        public DbProcedures(string connectionString): base(connectionString)
        {
        }

        /// <summary>
        /// Поиск городов по соответствию поисковому запросу.
        /// <remarks>Соответствие ищется с начала имени города.</remarks>
        /// </summary>
        /// <param name="query">Поисковый запрос - начало имени города.</param>
        /// <returns>Список найденных городов.</returns>
        public IList<CityInfo> SearchCity(string query)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = "dbo.api_SearchCity";
                cmd.CommandType = CommandType.StoredProcedure;

                var param = new SqlParameter("query", SqlDbType.NVarChar, 255) { Value = query };
                cmd.Parameters.Add(param);

                var result = ExecuteReader(cmd, CityInfoDbAdapter.GetCityInfo);

                return result;
            }
        }


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
        public RoutesInfoPackage SearchRouteForQuery(string query, int maxDistance, int offset, int rows)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = "dbo.api_GetDistancePageForQuery";
                cmd.CommandType = CommandType.StoredProcedure;

                var returnValue = new SqlParameter("ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(returnValue);

                var param = new SqlParameter("query", SqlDbType.NVarChar, 255) { Value = query };
                cmd.Parameters.Add(param);

                param = new SqlParameter("MaxDistance", SqlDbType.Int) { Value = maxDistance };
                cmd.Parameters.Add(param);

                param = new SqlParameter("offset", SqlDbType.Int) { Value = offset };
                cmd.Parameters.Add(param);

                param = new SqlParameter("rows", SqlDbType.Int) { Value = rows };
                cmd.Parameters.Add(param);

                return _executeCmdAndCreatePackage(cmd, returnValue);
            }
        }


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
        public RoutesInfoPackage SearchRouteForCity(long cityId, int maxDistance, int offset, int rows)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = "dbo.api_GetDistancePageForCity";
                cmd.CommandType = CommandType.StoredProcedure;

                var returnValue = new SqlParameter("ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };

                cmd.Parameters.Add(returnValue);

                var param = new SqlParameter("CityId", SqlDbType.BigInt) { Value = cityId };
                cmd.Parameters.Add(param);

                param = new SqlParameter("MaxDistance", SqlDbType.Int) { Value = maxDistance };
                cmd.Parameters.Add(param);

                param = new SqlParameter("Offset", SqlDbType.Int) { Value = offset };
                cmd.Parameters.Add(param);

                param = new SqlParameter("Rows", SqlDbType.Int) { Value = rows };
                cmd.Parameters.Add(param);

                return _executeCmdAndCreatePackage(cmd, returnValue);
            }
        }

        /// <summary>
        /// Обновления значения дистаниции в справочнике маршрутов.
        /// </summary>
        /// <param name="routeId">Id маршрута между двумя городами.</param>
        /// <param name="distance">Новое значение дистанции.</param>
        /// <returns>Количество обновлённых записей в БД.</returns>
        public int UpdateRouteDistance(long routeId, int distance)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = "dbo.api_UpdateRouteDistance";
                cmd.CommandType = CommandType.StoredProcedure;

                var param = new SqlParameter("routeId", SqlDbType.BigInt);
                param.Value = routeId;
                cmd.Parameters.Add(param);

                param = new SqlParameter("distance", SqlDbType.Int);
                param.Value = distance;
                cmd.Parameters.Add(param);

                var rowsAffected = ExecuteNonQuery(cmd);

                return rowsAffected;
            }
        }


        private RoutesInfoPackage _executeCmdAndCreatePackage(SqlCommand cmd, SqlParameter returnValueParameter)
        {
            var package = DataContractFactory.CreateRoutesInfoPackage();
            var routes = new List<RouteInfo>();
            var foundRoutesCount = ExecuteReaderWithReturnValue<RouteInfo, int>(cmd, RouteInfoDbAdapter.GetRouteInfo,
                                                                                routes, returnValueParameter);

            package.Routes.AddRange(routes);
            package.AllFoundRoutesCount = foundRoutesCount;

            return package;
        }
    }
}