using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using DistanceBtwCities.Common.Connections.Abstractions;

namespace DistanceBtwCities.Common.Connections.Implementation
{
    public abstract class ConnectionContext : IConnectionContext, IConnectionContextSettings
    {
        private readonly ConnectionContextData _connectionContextData;
        private IsolationLevel _transactionIsolationLevel;

        protected ConnectionContext(DbConnection connection)
        {
            _connectionContextData = new ConnectionContextData(connection);
            _transactionIsolationLevel = IsolationLevel.Unspecified;
        }

        public async Task<IConnectionContextData> GetContextDataAsync()
        {
            await TryOpenConnectionAsync()
                .ConfigureAwait(false);

            return _connectionContextData;
        }

        public void CommitTransaction()
        {
            _connectionContextData.Transaction?.Commit();
        }

        public void RollbackTransaction()
        {
            _connectionContextData.Transaction?.Rollback();
        }

        public void SetTransactionIsolationLevel(IsolationLevel level)
        {
            _transactionIsolationLevel = level;
        }

        private async Task TryOpenConnectionAsync()
        {
            if (_connectionContextData.Connection.State != ConnectionState.Closed)
                return;

            await _connectionContextData.Connection.OpenAsync()
                .ConfigureAwait(false);

            if (_transactionIsolationLevel == IsolationLevel.Unspecified)
                return;

            _connectionContextData.Transaction = _connectionContextData.Connection.BeginTransaction(_transactionIsolationLevel);
        }

        public void Dispose()
        {
            _connectionContextData.Connection.Dispose();
        }
    }
}
