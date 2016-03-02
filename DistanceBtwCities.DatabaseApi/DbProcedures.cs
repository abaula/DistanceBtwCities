using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DistanceBtwCities.Dal.Adapters;
using DistanceBtwCities.Dal.Contract;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Dal
{
    public class DbProcedures : IDbProcedures
    {
        private readonly string _connectionString;

        public DbProcedures(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<CityInfo> SearchCity(string query)
        {
            var result = new List<CityInfo>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "dbo.api_SearchCity";
                    cmd.CommandType = CommandType.StoredProcedure;

                    var param = new SqlParameter("query", SqlDbType.NVarChar, 255);
                    param.Value = query;
                    cmd.Parameters.Add(param);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var city = reader.GetCityInfo();
                            result.Add(city);
                        }
                    }
                }
            }

            return result;
        }


        public RoutesInfoPackage SearchRouteForQuery(string query, int maxDistance, int offset, int rows)
        {
            var package = DataContractFactory.CreateRoutesInfoPackage();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "dbo.api_GetDistancePageForQuery";
                    cmd.CommandType = CommandType.StoredProcedure;

                    var returnValue = new SqlParameter("ReturnValue", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);

                    var param = new SqlParameter("query", SqlDbType.NVarChar, 255);
                    param.Value = query;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter("MaxDistance", SqlDbType.Int);
                    param.Value = maxDistance;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter("offset", SqlDbType.Int);
                    param.Value = offset;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter("rows", SqlDbType.Int);
                    param.Value = rows;
                    cmd.Parameters.Add(param);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var route = reader.GetRouteInfo();
                            package.Routes.Add(route);
                        }

                        // обязательно закрываем reader, чтобы получить возвращённое значение
                        reader.Close();

                        package.AllFoundRoutesCount = Convert.ToInt32(returnValue.Value);
                    }
                }
            }

            return package;
        }


        public RoutesInfoPackage SearchRouteForCity(long cityId, int maxDistance, int offset, int rows)
        {
            var package = DataContractFactory.CreateRoutesInfoPackage();


            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "dbo.api_GetDistancePageForCity";
                    cmd.CommandType = CommandType.StoredProcedure;

                    var returnValue = new SqlParameter("ReturnValue", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);

                    var param = new SqlParameter("cityId", SqlDbType.BigInt);
                    param.Value = cityId;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter("MaxDistance", SqlDbType.Int);
                    param.Value = maxDistance;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter("offset", SqlDbType.Int);
                    param.Value = offset;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter("rows", SqlDbType.Int);
                    param.Value = rows;
                    cmd.Parameters.Add(param);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var route = reader.GetRouteInfo();
                            package.Routes.Add(route);
                        }

                        // обязательно закрываем reader, чтобы получить возвращённое значение
                        reader.Close();

                        package.AllFoundRoutesCount = Convert.ToInt32(returnValue.Value);
                    }
                }
            }

            return package;
        }

        public int UpdateRouteDistance(long routeId, int distance)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "dbo.api_UpdateRouteDistance";
                    cmd.CommandType = CommandType.StoredProcedure;

                    var param = new SqlParameter("routeId", SqlDbType.BigInt);
                    param.Value = routeId;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter("distance", SqlDbType.Int);
                    param.Value = distance;
                    cmd.Parameters.Add(param);

                    var rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected;
                }
            }
        }
    }
}