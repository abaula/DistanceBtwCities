using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DistanceBtwCities.Common.Connections.Abstractions;

namespace DistanceBtwCities.Dal.Common
{
    public class DaoBase
    {
        private readonly IConnectionContext _connectionContext;

        public DaoBase(IConnectionContext connectionContext)
        {
            _connectionContext = connectionContext;
        }

        protected DynamicParameters CreateDynamicParameters()
        {
            return new DynamicParameters();
        }

        protected async Task<IEnumerable<T>> Get<T>(string spName, DynamicParameters parameters = null)
        {
            var contextData = await _connectionContext.GetContextData()
                .ConfigureAwait(false);

            return await contextData.Connection.QueryAsync<T>(spName, 
                parameters, 
                commandType: CommandType.StoredProcedure, 
                transaction: contextData.Transaction)
                .ConfigureAwait(false);
        }

        protected async Task<int> Execute(string spName, DynamicParameters parameters = null)
        {
            var contextData = await _connectionContext.GetContextData()
                .ConfigureAwait(false);

            return await contextData.Connection.ExecuteAsync(spName, 
                parameters, 
                commandType: CommandType.StoredProcedure, 
                transaction: contextData.Transaction)
                .ConfigureAwait(false);
        }

        protected async Task<T> ExecuteScalar<T>(string spName, DynamicParameters parameters = null)
        {
            var contextData = await _connectionContext.GetContextData()
                .ConfigureAwait(false);

            return await contextData.Connection.ExecuteScalarAsync<T>(spName, 
                parameters, 
                commandType: CommandType.StoredProcedure,
                transaction: contextData.Transaction)
                .ConfigureAwait(false);
        }

        protected async Task<T> ReadMultiple<T>(string spName, DynamicParameters parameters, Func<SqlMapper.GridReader, Task<T>> func)
        {
            var contextData = await _connectionContext.GetContextData()
                .ConfigureAwait(false);

            var reader = await contextData.Connection.QueryMultipleAsync(spName,
                parameters,
                commandType: CommandType.StoredProcedure,
                transaction: contextData.Transaction)
                .ConfigureAwait(false);

            return await func(reader)
                .ConfigureAwait(false);
        }
    }
}
