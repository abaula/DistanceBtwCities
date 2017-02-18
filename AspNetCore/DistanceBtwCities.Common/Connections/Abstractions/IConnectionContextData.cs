using System.Data.Common;

namespace DistanceBtwCities.Common.Connections.Abstractions
{
    public interface IConnectionContextData
    {
        DbConnection Connection { get; }
        DbTransaction Transaction { get; }
    }
}
