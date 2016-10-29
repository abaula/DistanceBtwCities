using System.Data;
using System.Linq;
using DistanceBtwCities.Common.Abstractions.Dal;
using DistanceBtwCities.Common.Connections.Abstractions;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Common.Dtos.Requests;
using DistanceBtwCities.Dal.Common;
using DistanceBtwCities.Dal.DataObjects;
using DistanceBtwCities.Dal.Extensions;

namespace DistanceBtwCities.Dal.Queries
{
    public class SearchRouteQuery : DaoBase, IQuery<RouteSearchRequestDto, RoutesInfoPackage>
    {
        public SearchRouteQuery(IDistanceBtwCitiesConnection connection) : base(connection)
        {
        }

        public RoutesInfoPackage Ask(RouteSearchRequestDto request)
        {
            var parameters = CreateDynamicParameters();
            parameters.Add("@returnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@query", request.Query);
            parameters.Add("@maxDistance", request.MaxDistance);
            parameters.Add("@offset", request.Offset);
            parameters.Add("@rows", request.Rows);

            var routesInfos = Get<RouteInfoDo>("dbo.api_GetDistancePageForQuery", parameters)
                .Select(r => r.ToRouteInfo())
                .ToArray();

            return new RoutesInfoPackage
            {
                Routes = routesInfos,
                AllFoundRoutesCount = parameters.Get<int>("@returnValue")
            };
        }
    }
}
