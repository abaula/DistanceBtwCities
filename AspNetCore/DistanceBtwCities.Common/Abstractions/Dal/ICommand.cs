
namespace DistanceBtwCities.Common.Abstractions.Dal
{
    public interface ICommand<in TCmd>
    {
        void Execute(TCmd cmd);
    }
}
