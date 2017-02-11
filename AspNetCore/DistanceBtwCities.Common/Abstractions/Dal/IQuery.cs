using System.Threading.Tasks;

namespace DistanceBtwCities.Common.Abstractions.Dal
{
    public interface IQuery<in TRequest, TResult>
    {
        Task<TResult> Ask(TRequest request);
    }
}
