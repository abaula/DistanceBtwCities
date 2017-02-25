using System.Data.Common;
using DistanceBtwCities.Common.Connections.Abstractions;

namespace DistanceBtwCities.Common.Connections.Implementation
{
    public class ConnectionContextData : IConnectionContextData
    {
        public ConnectionContextData(DbConnection connection, DbTransaction transaction = null)
        {
            Connection = connection;
            Transaction = transaction;
        }

        public DbConnection Connection { get; }
        public DbTransaction Transaction { get; set; }
    }
}
