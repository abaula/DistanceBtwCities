using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DistanceBtwCities.Dal
{
    public class DbProceduresBase
    {
        public DbProceduresBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected string ConnectionString { get; }

        protected int ExecuteNonQuery(SqlCommand command)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                command.Connection = conn;
                var result = command.ExecuteNonQuery();
                conn.Close();

                return result;
            }
        }

        protected List<TResult> ExecuteReader<TResult>(SqlCommand command, Func<SqlDataReader, TResult> mappingFoo)
        {
            var result = new List<TResult>();

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                command.Connection = conn;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = mappingFoo(reader);
                        result.Add(obj);
                    }

                    reader.Close();
                }

                conn.Close();
            }

            return result;
        }

        protected TReturn ExecuteReaderWithReturnValue<TResult, TReturn>(SqlCommand command, Func<SqlDataReader, TResult> mappingFoo, List<TResult> list, SqlParameter returnParameter)
        {
            TReturn result;

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                command.Connection = conn;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = mappingFoo(reader);
                        list.Add(obj);
                    }

                    // обязательно закрываем reader, чтобы получить возвращённое значение
                    reader.Close();
                    result = (TReturn)Convert.ChangeType(returnParameter.Value, typeof(TReturn));
                }

                conn.Close();
            }

            return result;
        }
    }
}
