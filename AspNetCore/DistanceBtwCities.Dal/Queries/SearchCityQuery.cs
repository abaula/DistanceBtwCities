using System.Linq;
using System.Threading.Tasks;
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
        public SearchCityQuery(IDistanceBtwCitiesConnection connection) 
            : base(connection.Connection)
        {
        }

        public async Task<CityInfo[]> Ask(string request)
        {
            var parameters = CreateDynamicParameters();
            parameters.Add("@query", request);

            var data = await Get<CityInfoDo>("dbo.api_SearchCity", parameters);

            return data
                .Select(d => d.ToCityInfo())
                .ToArray();
        }
    }
}
