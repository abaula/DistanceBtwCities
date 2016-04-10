using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;

namespace DistanceBtwCities.Interception
{
    public class TransactionAttribute : InterceptAttribute
    {
        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            return new TransactionInterceptor();
        }
    }
}
