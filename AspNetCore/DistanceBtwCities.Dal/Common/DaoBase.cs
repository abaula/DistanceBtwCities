using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace DistanceBtwCities.Dal.Common
{
    public class DaoBase
    {
        protected SqlConnection Connection { get; }

        public DaoBase(SqlConnection connection)
        {
            Connection = connection;
        }

        protected DynamicParameters CreateDynamicParameters()
        {
            return new DynamicParameters();
        }

        protected async Task<IEnumerable<T>> Get<T>(string spName, DynamicParameters parameters = null)
        {
            return await Connection.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        protected async Task<int> Execute(string spName, DynamicParameters parameters = null)
        {
            return await Connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        protected async Task<T> ExecuteScalar<T>(string spName, DynamicParameters parameters = null)
        {
           return await Connection.ExecuteScalarAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        protected async Task<T> ReadMultiple<T>(string spName, DynamicParameters parameters, Func<Task<SqlMapper.GridReader>, Task<T>> func)
        {
            return await func(Connection.QueryMultipleAsync(spName, parameters, commandType: CommandType.StoredProcedure));
        }
    }
}
