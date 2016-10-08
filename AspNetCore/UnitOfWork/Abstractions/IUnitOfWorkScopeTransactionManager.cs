using System;

namespace UnitOfWork.Abstractions
{
    public interface IUnitOfWorkScopeTransactionManager
    {
        void RegisterAndBeginTransaction(Type scopeWorkerType);
        void DoCommit();
    }
}
