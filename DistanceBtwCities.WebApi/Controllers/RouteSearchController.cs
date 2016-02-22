﻿using System.Threading.Tasks;
using System.Web.Http;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model;
using DistanceBtwCities.Model.Contract;
using DistanceBtwCities.WebApi.Properties;

namespace DistanceBtwCities.WebApi.Controllers
{
    [RoutePrefix("api/searchroute")]
    public class RouteSearchController : ApiController
    {
        [HttpGet]
        [Route("query/{query:maxlength(255)}/{MaxDistance:int:min(0)}/{offset:int:min(0)}/{rows:int:min(1)}")]
        public async Task<RoutesInfoPackage> SearchRouteForQuery(string query, int maxDistance, int offset, int rows)
        {
            var searchRouteTask = _getSearchRouteTask();

            await searchRouteTask.SearchRouteForQuery(query, maxDistance, offset, rows);

            return searchRouteTask.Package;
        }

        [HttpGet]
        [Route("query/{MaxDistance:int:min(0)}/{offset:int:min(0)}/{rows:int:min(1)}")]
        public async Task<RoutesInfoPackage> SearchRouteForEmptyQuery(int maxDistance, int offset, int rows)
        {
            var searchRouteTask = _getSearchRouteTask();

            await searchRouteTask.SearchRouteForQuery(string.Empty, maxDistance, offset, rows);

            return searchRouteTask.Package;
        }


        [HttpGet]
        [Route("city/{cityId:long:min(1)}/{MaxDistance:int:min(0)}/{offset:int:min(0)}/{rows:int:min(1)}")]
        public async Task<RoutesInfoPackage> SearchRouteForCity(long cityId, int maxDistance, int offset, int rows)
        {
            var searchRouteTask = _getSearchRouteTask();

            await searchRouteTask.SearchRouteForCity(cityId, maxDistance, offset, rows);

            return searchRouteTask.Package;
        }

        private ISearchRouteTask _getSearchRouteTask()
        {
            var factory = ModelFactory.CreateInstance(Settings.Default.DbConnectionString);
            var searchRouteTask = factory.CreateSearchRouteTask();

            return searchRouteTask;
        }
    }
}