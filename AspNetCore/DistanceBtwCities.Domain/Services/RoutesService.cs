using System.Data;
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

        public async Task<RoutesInfoPackage> SearchRouteAsync(RouteSearchRequestDto request)
        {
            using (var scope = _unitOfWorkFactory.CreateScope())
            {
                if (request.CityId != null)
                {
                    return await scope.Get<IQuery<RouteSearchRequestCityDto, RoutesInfoPackage>>()
                        .AskAsync(request)
                        .ConfigureAwait(false);
                }

                return await scope.Get<IQuery<RouteSearchRequestDto, RoutesInfoPackage>>()
                    .AskAsync(request)
                    .ConfigureAwait(false);
            }
        }

        public async Task UpdateRouteDistanceAsync(RouteUpdateInfo request)
        {
            using (var scope = _unitOfWorkFactory.CreateTransactionScope(IsolationLevel.ReadCommitted))
            {
                await scope.Get<ICommand<RouteUpdateInfo>>()
                    .ExecuteAsync(request)
                    .ConfigureAwait(false);

                scope.Commit();
            }
        }
    }
}
