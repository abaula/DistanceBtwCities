
namespace UnitOfWork.Abstractions
{
    internal interface IRepeatableReadSupport
    {
        bool UseRepeatableRead { get; set; }
    }
}
