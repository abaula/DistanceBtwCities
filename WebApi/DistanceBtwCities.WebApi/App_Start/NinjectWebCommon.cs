using DistanceBtwCities.Dal;
using DistanceBtwCities.Dal.Contract;
using DistanceBtwCities.Model;
using DistanceBtwCities.Model.Contract;
using DistanceBtwCities.WebApi.Caching;
using DistanceBtwCities.WebApi.Properties;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DistanceBtwCities.WebApi.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DistanceBtwCities.WebApi.NinjectWebCommon), "Stop")]

namespace DistanceBtwCities.WebApi
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static IKernel Kernel => bootstrapper.Kernel;
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<CacheSegmentsComparer>().ToSelf();
            kernel.Bind<ICacheSettingsManager>().To<CacheSettingsManager>().InSingletonScope();
            kernel.Bind<ICacheManager>().To<CacheManager>().InSingletonScope();
            kernel.Bind<ISearchCityService>().To<SearchCityService>().InRequestScope();
            kernel.Bind<IRouteEditService>().To<RouteEditService>().InRequestScope();
            kernel.Bind<ISearchRouteService>().To<SearchRouteService>().InRequestScope();

            kernel.Bind<IDbProcedures>().To<DbProcedures>().InRequestScope()
                .WithConstructorArgument("connectionString", Settings.Default.DbConnectionString);
        }        
    }
}
