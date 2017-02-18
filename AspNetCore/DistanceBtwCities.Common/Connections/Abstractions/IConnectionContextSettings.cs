
using System.Data;

namespace DistanceBtwCities.Common.Connections.Abstractions
{
    public interface IConnectionContextSettings
    {
        void SetTransactionIsolationLevel(IsolationLevel level);
    }
}
