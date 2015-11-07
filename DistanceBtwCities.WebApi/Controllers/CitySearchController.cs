using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.WebApi.Controllers
{    
    [RoutePrefix("api/searchcity")]
    public class CitySearchController : ApiController
    {
        [HttpGet]
        [Route("{query:maxlength(255)}")]
        public async Task<IEnumerable<CityInfo>> SearchCity(string query)
        {
            var searchCityTask = _getSearchCityTask();

            await searchCityTask.SearchCityAsync(query);

            return searchCityTask.Cities;
        }

        ISearchCityTask _getSearchCityTask()
        {
            var factory = ModelFactory.CreateInstance(Properties.Settings.Default.DbConnectionString);
            var searchCityTask = factory.CreateSearchCityTask();

            return searchCityTask;
        }
    }
}
