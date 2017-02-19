using System;
using System.Threading.Tasks;

namespace DistanceBtwCities.Common.Connections.Abstractions
{
    public interface IConnectionContext : IDisposable
    {
        Task<IConnectionContextData> GetContextDataAsync();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
