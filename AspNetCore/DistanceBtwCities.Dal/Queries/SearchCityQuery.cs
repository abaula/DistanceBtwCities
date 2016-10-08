using System.Linq;
using AutoMapper;
using DistanceBtwCities.Common.Abstractions.Dal;
using DistanceBtwCities.Common.Connections.Abstractions;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Dal.Common;
using DistanceBtwCities.Dal.DataObjects;

namespace DistanceBtwCities.Dal.Queries
{
    public class SearchCityQuery : DaoBase, IQuery<string, CityInfo[]>
    {
        private readonly IMapper _mapper;

        public SearchCityQuery(IDistanceBtwCitiesConnection connection, IMapper mapper) : base(connection)
        {
            _mapper = mapper;
        }

        public CityInfo[] Ask(string request)
        {
            var parameters = CreateDynamicParameters();
            parameters.Add("@query", request);

            return Get<CityInfoDo>("dbo.api_SearchCity", parameters)
                .Select(_mapper.Map<CityInfo>)
                .ToArray();
        }
    }
}
