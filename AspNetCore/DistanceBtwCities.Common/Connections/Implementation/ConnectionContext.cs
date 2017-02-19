using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using DistanceBtwCities.Common.Connections.Abstractions;

namespace DistanceBtwCities.Common.Connections.Implementation
{
    public abstract class ConnectionContext : IConnectionContext, IConnectionContextSettings
    {
        private readonly DbConnection _connection;
        private DbTransaction _transaction;
        private IsolationLevel _transactionIsolationLevel;

        protected ConnectionContext(DbConnection connection)
        {
            _connection = connection;
            _transactionIsolationLevel = IsolationLevel.Unspecified;
        }

        public async Task<IConnectionContextData> GetContextData()
        {
            await TryOpenConnection()
                .ConfigureAwait(false);
            return new ConnectionContextData(_connection, _transaction);
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
        }

        public void SetTransactionIsolationLevel(IsolationLevel level)
        {
            _transactionIsolationLevel = level;
        }

        private async Task TryOpenConnection()
        {
            if (_connection.State != ConnectionState.Closed)
                return;

            await _connection.OpenAsync()
                .ConfigureAwait(false);

            if (_transactionIsolationLevel == IsolationLevel.Unspecified)
                return;

            _transaction = _connection.BeginTransaction(_transactionIsolationLevel);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
