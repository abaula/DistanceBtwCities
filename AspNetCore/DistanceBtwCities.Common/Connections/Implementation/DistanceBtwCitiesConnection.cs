using System.Data.SqlClient;
using DistanceBtwCities.Common.Connections.Abstractions;

namespace DistanceBtwCities.Common.Connections.Implementation
{
    public class DistanceBtwCitiesConnection : AppConnectionBase, IDistanceBtwCitiesConnection
    {
        public DistanceBtwCitiesConnection(string connectionString)
            : base(new SqlConnection(connectionString))
        {
        }
    }
}
