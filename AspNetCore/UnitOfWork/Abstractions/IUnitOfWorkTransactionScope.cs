
using System;
using System.Data;

namespace UnitOfWork.Abstractions
{
    public interface IUnitOfWorkTransactionScope : IDisposable
    {
        void SetIsolationLevel(IsolationLevel isolationlevel);
        T Get<T>();
        void Commit();
    }
}
