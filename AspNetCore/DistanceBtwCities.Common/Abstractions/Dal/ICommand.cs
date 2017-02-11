
using System.Threading.Tasks;

namespace DistanceBtwCities.Common.Abstractions.Dal
{
    public interface ICommand<in TCmd>
    {
        Task Execute(TCmd cmd);
    }
}
