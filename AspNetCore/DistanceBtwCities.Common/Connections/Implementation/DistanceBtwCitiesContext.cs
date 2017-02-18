using System.Data.SqlClient;
using DistanceBtwCities.Common.Connections.Abstractions;

namespace DistanceBtwCities.Common.Connections.Implementation
{
    public class DistanceBtwCitiesContext : ConnectionContext, IDistanceBtwCitiesContext
    {
        public DistanceBtwCitiesContext(string connectionString)
            : base(new SqlConnection(connectionString))
        {
        }
    }
}
