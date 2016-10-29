using System.Linq;
using DistanceBtwCities.Common.Abstractions.Dal;
using DistanceBtwCities.Common.Connections.Abstractions;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Dal.Common;
using DistanceBtwCities.Dal.DataObjects;
using DistanceBtwCities.Dal.Extensions;

namespace DistanceBtwCities.Dal.Queries
{
    public class SearchCityQuery : DaoBase, IQuery<string, CityInfo[]>
    {
        public SearchCityQuery(IDistanceBtwCitiesConnection connection) : base(connection)
        {
        }

        public CityInfo[] Ask(string request)
        {
            var parameters = CreateDynamicParameters();
            parameters.Add("@query", request);

            return Get<CityInfoDo>("dbo.api_SearchCity", parameters)
                .Select(d => d.ToCityInfo())
                .ToArray();
        }
    }
}
