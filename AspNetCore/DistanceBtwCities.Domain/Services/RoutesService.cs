using DistanceBtwCities.Common.Abstractions.Dal;
using DistanceBtwCities.Common.Abstractions.Domain;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Common.Dtos.Requests;
using UnitOfWork.Abstractions;

namespace DistanceBtwCities.Domain.Services
{
    public class RoutesService : IRoutesService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public RoutesService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public RoutesInfoPackage SearchRoute(RouteSearchRequestDto request)
        {
            using (var scope = _unitOfWorkFactory.CreateScope())
            {
                if (request.CityId != null)
                {
                    return scope.Get<IQuery<RouteSearchRequestCityDto, RoutesInfoPackage>>()
                        .Ask(request);
                }

                return scope.Get<IQuery<RouteSearchRequestDto, RoutesInfoPackage>>()
                    .Ask(request);
            }
        }

        public RouteInfo UpdateRouteDistance(RouteInfo request)
        {
            using (var scope = _unitOfWorkFactory.CreateTransactionScope())
            {
                scope.Get<ICommand<RouteInfo>>()
                    .Execute(request);

                scope.Commit();

                return request;
            }
        }
    }
}
