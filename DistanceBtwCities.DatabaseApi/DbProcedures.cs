using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistanceBtwCities.DatabaseApi.Adapters;
using DistanceBtwCities.DatabaseApi.Constants;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.DatabaseApi
{
    public class DbProcedures
    {
        private string _connectionString;

        internal DbProcedures(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CityInfo> SearchCity(string query)
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

                    param = new SqlParameter("maxDistance", SqlDbType.Int);
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

                    param = new SqlParameter("maxDistance", SqlDbType.Int);
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
    }
}
