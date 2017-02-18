using System;
using System.Data;

namespace UnitOfWork.Abstractions
{
    public interface IUnitOfWorkScopeTransactionManager
    {
        void CheckAndRegisterConnectionContext(Type scopeWorkerType, IsolationLevel isolationLevel);
        void DoCommit();
    }
}
