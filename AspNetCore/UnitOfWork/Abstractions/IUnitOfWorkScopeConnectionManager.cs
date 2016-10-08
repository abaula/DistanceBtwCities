using System;

namespace UnitOfWork.Abstractions
{
    public interface IUnitOfWorkScopeConnectionManager
    {
        void RegisterConnection(Type scopeWorkerType);
    }
}
