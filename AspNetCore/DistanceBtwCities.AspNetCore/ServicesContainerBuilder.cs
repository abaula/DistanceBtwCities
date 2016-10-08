using Autofac;
using AutoMapper;
using DistanceBtwCities.Common.Abstractions.Dal;
using DistanceBtwCities.Common.Abstractions.Domain;
using DistanceBtwCities.Common.Connections.Abstractions;
using DistanceBtwCities.Common.Connections.Implementation;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Common.Dtos.Requests;
using DistanceBtwCities.Dal.Commands;
using DistanceBtwCities.Dal.Queries;
using DistanceBtwCities.Domain.Services;
using Microsoft.Extensions.Configuration;
using UnitOfWork.Abstractions;
using UnitOfWork.Implementation;

namespace DistanceBtwCities.AspNetCore
{
    public static class ServicesContainerBuilder
    {
        public static ContainerBuilder Get(IConfigurationRoot configuration)
        {
            var builder = new ContainerBuilder();

            // AutoMapper
            var autoMapperInstance = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperDapperProfile>())
                .CreateMapper();
            builder.RegisterInstance(autoMapperInstance).As<IMapper>();

            // CQRS
            builder.RegisterType<DistanceBtwCitiesConnection>().As<IDistanceBtwCitiesConnection>()
                .WithParameter(new TypedParameter(typeof(string), configuration["ConnectionString"]));
            builder.RegisterType<UnitOfWorkTransactionScope>().As<IUnitOfWorkTransactionScope>();
            builder.RegisterType<UnitOfWorkScope>().As<IUnitOfWorkScope>();
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            builder.RegisterType<UnitOfWorkScopeTransactionManager>().As<IUnitOfWorkScopeTransactionManager>();
            builder.RegisterType<UnitOfWorkScopeConnectionManager>().As<IUnitOfWorkScopeConnectionManager>();
            // CQRS worker-ы обязательно регистрируем с опцией InstancePerLifetimeScope().
            builder.RegisterType<SearchCityQuery>().As<IQuery<string, CityInfo[]>>().InstancePerLifetimeScope();
            builder.RegisterType<SearchRouteQuery>().As<IQuery<RouteSearchRequestDto, RoutesInfoPackage>>().InstancePerLifetimeScope();
            builder.RegisterType<SearchRouteByCityQuery>().As<IQuery<RouteSearchRequestCityDto, RoutesInfoPackage>>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateRouteCmd>().As<ICommand<RouteUpdateDistanceRequestDto>>().InstancePerLifetimeScope();


            // Сервисы
            builder.RegisterType<CitiesService>().As<ICitiesService>();
            builder.RegisterType<RoutesService>().As<IRoutesService>();



            return builder;
        }
    }
}
