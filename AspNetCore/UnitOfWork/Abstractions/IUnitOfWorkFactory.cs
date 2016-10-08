
namespace UnitOfWork.Abstractions
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWorkTransactionScope CreateTransactionScope();
        IUnitOfWorkScope CreateScope();
    }
}
