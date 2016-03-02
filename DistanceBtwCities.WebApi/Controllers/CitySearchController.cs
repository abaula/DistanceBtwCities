using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.WebApi.Controllers
{
    [RoutePrefix("api/searchcity")]
    public class CitySearchController : ApiController
    {
        private readonly ISearchCityService _searchCityService;

        public CitySearchController(ISearchCityService searchCityService)
        {
            _searchCityService = searchCityService;
        }

        [HttpGet]
        [Route("{query:maxlength(255)}")]
        public async Task<IList<CityInfo>> SearchCity(string query)
        {
            var cities = await _searchCityService.SearchCityAsync(query);

            return cities;
        }
    }
}