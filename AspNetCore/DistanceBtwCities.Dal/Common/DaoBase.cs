using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace DistanceBtwCities.Dal.Common
{
    public class DaoBase
    {
        protected IDbConnection Connection { get; }

        public DaoBase(IDbConnection connection)
        {
            Connection = connection;
        }

        protected DynamicParameters CreateDynamicParameters()
        {
            return new DynamicParameters();
        }

        protected IEnumerable<T> Get<T>(string spName, DynamicParameters parameters = null)
        {
            return Connection.Query<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        protected int Execute(string spName, DynamicParameters parameters = null)
        {
            return Connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        protected T ExecuteScalar<T>(string spName, DynamicParameters parameters = null)
        {
           return Connection.ExecuteScalar<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        protected T ReadMultiple<T>(string spName, DynamicParameters parameters, Func<SqlMapper.GridReader, T> func)
        {
            return func(Connection.QueryMultiple(spName, parameters, commandType: CommandType.StoredProcedure));
        }
    }
}
