using System.Web.Http;
using DistanceBtwCities.WebApi.Handlers;

namespace DistanceBtwCities.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional}
                );

            // добавляем общие обработчики сообщений для всех HttpRoute
            config.MessageHandlers.Add(new DelegatingHandlerProxy<MessageHandlerForCaching>(NinjectWebCommon.Kernel));
        }
    }
}