using System.Data;
using System.Data.Common;

namespace DistanceBtwCities.Common.Connections.Implementation
{
    public abstract class AppConnectionBase : DbConnection
    {
        private readonly DbConnection _connectionObject;

        protected AppConnectionBase(DbConnection connectionObject)
        {
            _connectionObject = connectionObject;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return _connectionObject.BeginTransaction(isolationLevel);
        }

        public override void Close()
        {
            _connectionObject.Close();
        }

        public override void Open()
        {
            _connectionObject.Open();
        }

        public override string ConnectionString
        {
            get { return _connectionObject.ConnectionString; }
            set { _connectionObject.ConnectionString = value; }
        }

        public override string Database => _connectionObject.Database;
        public override ConnectionState State => _connectionObject.State;
        public override string DataSource => _connectionObject.DataSource;
        public override string ServerVersion => _connectionObject.ServerVersion;

        protected override DbCommand CreateDbCommand()
        {
            return _connectionObject.CreateCommand();
        }

        public override void ChangeDatabase(string databaseName)
        {
            _connectionObject.ChangeDatabase(databaseName);
        }
    }
}
