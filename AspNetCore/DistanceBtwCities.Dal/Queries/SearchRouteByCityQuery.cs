using System;
using System.Data;
using System.Linq;
using AutoMapper;
using DistanceBtwCities.Common.Abstractions.Dal;
using DistanceBtwCities.Common.Connections.Abstractions;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Common.Dtos.Requests;
using DistanceBtwCities.Dal.Common;
using DistanceBtwCities.Dal.DataObjects;

namespace DistanceBtwCities.Dal.Queries
{
    public class SearchRouteByCityQuery : DaoBase, IQuery<RouteSearchRequestCityDto, RoutesInfoPackage>
    {
        private readonly IMapper _mapper;

        public SearchRouteByCityQuery(IDistanceBtwCitiesConnection connection, IMapper mapper) : base(connection)
        {
            _mapper = mapper;
        }

        public RoutesInfoPackage Ask(RouteSearchRequestCityDto request)
        {
            var parameters = CreateDynamicParameters();
            parameters.Add("@returnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@cityId", request.CityId);
            parameters.Add("@maxDistance", request.MaxDistance);
            parameters.Add("@offset", request.Offset);
            parameters.Add("@rows", request.Rows);

            var routesInfos = Get<RouteInfoDo>("dbo.api_GetDistancePageForCity", parameters)
                .Select(_mapper.Map<RouteInfo>)
                .ToArray();

            return new RoutesInfoPackage
            {
                Routes = routesInfos,
                AllFoundRoutesCount = parameters.Get<int>("@returnValue")
            };
        }
    }
}
