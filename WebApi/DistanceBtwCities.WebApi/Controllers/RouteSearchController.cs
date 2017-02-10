using System.Web.Http;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.WebApi.Controllers
{
    [RoutePrefix("api/searchroute")]
    public class RouteSearchController : ApiController
    {
        private readonly ISearchRouteService _searchRouteService;

        public RouteSearchController(ISearchRouteService searchRouteService)
        {
            _searchRouteService = searchRouteService;
        }

        [HttpGet]
        [Route("query/{query:maxlength(255)}/{MaxDistance:int:min(0)}/{offset:int:min(0)}/{rows:int:min(1)}")]
        public RoutesInfoPackage SearchRouteForQuery(string query, int maxDistance, int offset, int rows)
        {
            return _searchRouteService.SearchRouteForQuery(query, maxDistance, offset, rows);
        }

        [HttpGet]
        [Route("query/{MaxDistance:int:min(0)}/{offset:int:min(0)}/{rows:int:min(1)}")]
        public RoutesInfoPackage SearchRouteForEmptyQuery(int maxDistance, int offset, int rows)
        {
            return _searchRouteService.SearchRouteForQuery(string.Empty, maxDistance, offset, rows);
        }

        [HttpGet]
        [Route("city/{cityId:long:min(1)}/{MaxDistance:int:min(0)}/{offset:int:min(0)}/{rows:int:min(1)}")]
        public RoutesInfoPackage SearchRouteForCity(long cityId, int maxDistance, int offset, int rows)
        {
            return _searchRouteService.SearchRouteForCity(cityId, maxDistance, offset, rows);
        }
    }
}