using System.Data;
using System.Data.SqlClient;

namespace DistanceBtwCities.Common.Connections.Implementation
{
    public class DistanceBtwCitiesSqlConnectionBase : IDbConnection
    {
        private readonly SqlConnection _connection;

        public DistanceBtwCitiesSqlConnectionBase(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public IDbTransaction BeginTransaction()
        {
            return ((IDbConnection) _connection).BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return ((IDbConnection) _connection).BeginTransaction(il);
        }

        public void Close()
        {
            _connection.Close();
        }

        public IDbCommand CreateCommand()
        {
            return ((IDbConnection) _connection).CreateCommand();
        }

        public void Open()
        {
            _connection.Open();
        }

        public string ConnectionString
        {
            get { return _connection.ConnectionString; }
            set { _connection.ConnectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return _connection.ConnectionTimeout; }
        }

        public string Database
        {
            get { return _connection.Database; }
        }

        public ConnectionState State
        {
            get { return _connection.State; }
        }

        public void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }
    }
}
