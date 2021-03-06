﻿using System.Linq;
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
        public SearchCityQuery(IDistanceBtwCitiesContext context) 
            : base(context)
        {
        }

        public async Task<CityInfo[]> AskAsync(string request)
        {
            var parameters = CreateDynamicParameters();
            parameters.Add("@query", request);

            var data = await GetAsync<CityInfoDo>("dbo.api_SearchCity", parameters)
                .ConfigureAwait(false);

            return data
                .Select(d => d.ToCityInfo())
                .ToArray();
        }
    }
}
