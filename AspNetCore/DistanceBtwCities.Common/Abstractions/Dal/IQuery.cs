
namespace DistanceBtwCities.Common.Abstractions.Dal
{
    public interface IQuery<in TRequest, out TResult>
    {
        TResult Ask(TRequest request);
    }
}
