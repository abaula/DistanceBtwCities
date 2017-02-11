using System.Data.SqlClient;

namespace DistanceBtwCities.Common.Connections.Abstractions
{
    public interface IAppCommonConnection
    {
        SqlConnection Connection { get; }
    }
}
