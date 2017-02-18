using System;

namespace UnitOfWork.Abstractions
{
    public interface IUnitOfWorkScopeConnectionManager
    {
        void CheckAndRegisterConnectionContext(Type scopeWorkerType);
    }
}
