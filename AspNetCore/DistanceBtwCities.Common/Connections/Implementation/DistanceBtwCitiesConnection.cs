using System.Data.SqlClient;
using DistanceBtwCities.Common.Connections.Abstractions;

namespace DistanceBtwCities.Common.Connections.Implementation
{
    public class DistanceBtwCitiesConnection : IDistanceBtwCitiesConnection
    {
        public DistanceBtwCitiesConnection(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        public SqlConnection Connection { get; }
    }
}
