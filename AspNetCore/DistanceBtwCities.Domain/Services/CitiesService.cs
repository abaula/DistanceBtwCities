using System.Collections.Generic;
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

        public IEnumerable<CityInfo> SearchCity(string query)
        {
            using (var scope = _unitOfWorkFactory.CreateScope())
            {
                var worker = scope.Get<IQuery<string, CityInfo[]>>();
                var result = worker.Ask(query);
                return result;
            }
        }
    }
}
