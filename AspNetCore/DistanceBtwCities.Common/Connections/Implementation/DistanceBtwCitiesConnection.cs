using DistanceBtwCities.Common.Connections.Abstractions;

namespace DistanceBtwCities.Common.Connections.Implementation
{
    public class DistanceBtwCitiesConnection : DistanceBtwCitiesSqlConnectionBase, IDistanceBtwCitiesConnection
    {
        public DistanceBtwCitiesConnection(string connectionString)
            : base(connectionString)
        {
        }
    }
}
