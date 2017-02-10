using System.Collections.Generic;
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
        public IList<CityInfo> SearchCity(string query)
        {
            return _searchCityService.SearchCityAsync(query);
        }
    }
}