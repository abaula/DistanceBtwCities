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

        /// <summary>
        /// Строка подключения к БД.
        /// </summary>
        protected string ConnectionString { get; }

        /// <summary>
        /// Выполнение запроса в БД с использованием метода ADO.NET ExecuteNonQuery.
        /// </summary>
        /// <param name="command">Команда для выполнения.</param>
        /// <returns>Количество изменённых записей в БД.</returns>
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

        /// <summary>
        /// Загрузка из БД значений указанного типа в список.
        /// </summary>
        /// <typeparam name="TResult">Тип значения в списке.</typeparam>
        /// <param name="command">Команда для выполнения.</param>
        /// <param name="mappingFoo">Метод мапинга из записи типа SqlDataReader в тип <typeparamref name="TResult"/>.</param>
        /// <returns>Список объектов типа <typeparamref name="TResult"/>.</returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult">Тип значения в списке.</typeparam>
        /// <typeparam name="TReturn">Тип возвращаемого значения.</typeparam>
        /// <param name="command">Команда для выполнения.</param>
        /// <param name="mappingFoo">Метод мапинга из записи типа SqlDataReader в тип <typeparamref name="TResult"/>.</param>
        /// <param name="list">Список объектов типа <typeparamref name="TResult"/>.</param>
        /// <param name="returnParameter">SqlParameter типа <typeparamref name="TReturn"/> содержащий возвращаемое командой значение.</param>
        /// <returns>Полученное значение параметра <paramref name="returnParameter"/>.</returns>
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
