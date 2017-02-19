using System.Collections.Generic;
using System.Threading.Tasks;
using DistanceBtwCities.Common.Abstractions.Dal;
using DistanceBtwCities.Common.Abstractions.Domain;
using DistanceBtwCities.Common.Dtos;
using UnitOfWork.Abstractions;

namespace DistanceBtwCities.Domain.Services
{
    public class CitiesService : ICitiesService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public CitiesService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<IEnumerable<CityInfo>> SearchCity(string query)
        {
            using (var scope = _unitOfWorkFactory.CreateScope())
            {
                return await scope.Get<IQuery<string, CityInfo[]>>()
                    .Ask(query)
                    .ConfigureAwait(false);
            }
        }
    }
}
