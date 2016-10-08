
using System;

namespace UnitOfWork.Abstractions
{
    public interface IUnitOfWorkTransactionScope : IDisposable
    {
        T Get<T>();
        void Commit();
    }
}
