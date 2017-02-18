using System.Data;

namespace UnitOfWork.Abstractions
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWorkTransactionScope CreateTransactionScope(IsolationLevel isolationLevel);
        IUnitOfWorkScope CreateScope();
    }
}
