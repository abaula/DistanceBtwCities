using System;
using System.Threading.Tasks;

namespace DistanceBtwCities.Common.Connections.Abstractions
{
    public interface IConnectionContext : IDisposable
    {
        Task<IConnectionContextData> GetContextData();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
