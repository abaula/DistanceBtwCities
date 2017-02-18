using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DistanceBtwCities.Common.Abstractions.Dal;
using DistanceBtwCities.Common.Connections.Abstractions;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Common.Dtos.Requests;
using DistanceBtwCities.Dal.Common;
using DistanceBtwCities.Dal.DataObjects;
using DistanceBtwCities.Dal.Extensions;

namespace DistanceBtwCities.Dal.Queries
{
    public class SearchRouteByCityQuery : DaoBase, IQuery<RouteSearchRequestCityDto, RoutesInfoPackage>
    {
        public SearchRouteByCityQuery(IDistanceBtwCitiesContext context) 
            : base(context)
        {
        }

        public async Task<RoutesInfoPackage> Ask(RouteSearchRequestCityDto request)
        {
            var parameters = CreateDynamicParameters();
            parameters.Add("@returnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@cityId", request.CityId);
            parameters.Add("@maxDistance", request.MaxDistance);
            parameters.Add("@offset", request.Offset);
            parameters.Add("@rows", request.Rows);

            var data = await Get<RouteInfoDo>("dbo.api_GetDistancePageForCity", parameters);
            var routesInfos = data.Select(r => r.ToRouteInfo())
                .ToArray();

            return new RoutesInfoPackage
            {
                Routes = routesInfos,
                AllFoundRoutesCount = parameters.Get<int>("@returnValue")
            };
        }
    }
}
