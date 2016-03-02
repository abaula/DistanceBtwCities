using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ninject;

namespace DistanceBtwCities.WebApi
{
    public class DelegatingHandlerProxy<TDelegatingHandler> : DelegatingHandler where TDelegatingHandler : DelegatingHandler
    {
        private readonly IKernel _kernel;

        public DelegatingHandlerProxy(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // trigger the creation of the scope.
            request.GetDependencyScope();

            var handler = _kernel.Get<TDelegatingHandler>();

            handler.InnerHandler = this.InnerHandler;

            var invoker = new HttpMessageInvoker(handler);

            var response = await invoker.SendAsync(request, cancellationToken);

            _kernel.Release(handler);

            return response;
        }
    }
}