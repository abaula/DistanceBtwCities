using System.Threading.Tasks;
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

        public async Task<RoutesInfoPackage> SearchRoute(RouteSearchRequestDto request)
        {
            using (var scope = _unitOfWorkFactory.CreateScope())
            {
                if (request.CityId != null)
                {
                    return await scope.Get<IQuery<RouteSearchRequestCityDto, RoutesInfoPackage>>()
                        .Ask(request);
                }

                return await scope.Get<IQuery<RouteSearchRequestDto, RoutesInfoPackage>>()
                    .Ask(request);
            }
        }

        public async Task<RouteInfo> UpdateRouteDistance(RouteInfo request)
        {
            using (var scope = _unitOfWorkFactory.CreateTransactionScope())
            {
                await scope.Get<ICommand<RouteInfo>>()
                    .Execute(request);

                scope.Commit();

                return request;
            }
        }
    }
}
