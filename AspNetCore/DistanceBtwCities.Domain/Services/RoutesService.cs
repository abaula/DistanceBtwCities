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
                    var cityQuery = scope.Get<IQuery<RouteSearchRequestCityDto, RoutesInfoPackage>>();
                    return cityQuery.Ask(request);
                }

                var query = scope.Get<IQuery<RouteSearchRequestDto, RoutesInfoPackage>>();
                return query.Ask(request);
            }
        }

        public void UpdateRouteDistance(RouteUpdateDistanceRequestDto request)
        {
            using (var scope = _unitOfWorkFactory.CreateTransactionScope())
            {
                var cmd = scope.Get<ICommand<RouteUpdateDistanceRequestDto>>();
                cmd.Execute(request);
                scope.Commit();                
            }
        }
    }
}
