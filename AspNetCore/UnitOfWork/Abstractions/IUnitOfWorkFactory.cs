
namespace UnitOfWork.Abstractions
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWorkTransactionScope CreateTransactionScope(bool useRepeatableRead = false);
        IUnitOfWorkScope CreateScope();
    }
}
