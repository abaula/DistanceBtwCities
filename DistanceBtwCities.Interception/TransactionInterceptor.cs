using System.Transactions;
using Ninject.Extensions.Interception;

namespace DistanceBtwCities.Interception
{
    public class TransactionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            using (var transactionScope = new TransactionScope())
            {
                invocation.Proceed();
                transactionScope.Complete();
            }
        }
    }
}
