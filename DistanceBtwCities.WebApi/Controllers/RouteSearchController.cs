using System.Threading.Tasks;
using System.Web.Http;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.WebApi.Controllers
{
    [RoutePrefix("api/searchroute")]
    public class RouteSearchController : ApiController
    {
        [HttpGet]
        [Route("{query:maxlength(255)}/{maxDistance:int:min(0)}/{offset:int:min(0)}/{rows:int:min(1)}")]
        public async Task<RoutesInfoPackage> SearchRouteForQuery(string query, int maxDistance, int offset, int rows)
        {
            var searchRouteTask = _getSearchRouteTask();

            await searchRouteTask.SearchRouteForQuery(query, maxDistance, offset, rows);

            return searchRouteTask.Package;
        }

        [HttpGet]
        [Route("{cityId:long:min(1)}/{maxDistance:int:min(0)}/{offset:int:min(0)}/{rows:int:min(1)}")]
        public async Task<RoutesInfoPackage> SearchRouteForCity(long cityId, int maxDistance, int offset, int rows)
        {
            var searchRouteTask = _getSearchRouteTask();

            await searchRouteTask.SearchRouteForCity(cityId, maxDistance, offset, rows);

            return searchRouteTask.Package;
        }

        ISearchRouteTask _getSearchRouteTask()
        {
            var factory = ModelFactory.CreateInstance(Properties.Settings.Default.DbConnectionString);
            var searchRouteTask = factory.CreateSearchRouteTask();

            return searchRouteTask;
        }
    }
}