using System;

namespace UnitOfWork.Abstractions
{
    public interface IUnitOfWorkScope : IDisposable
    {
        T Get<T>();
    }
}
