using System.Threading.Tasks;
using System.Web.Http;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model;
using DistanceBtwCities.Model.Contract;
using DistanceBtwCities.WebApi.Properties;

namespace DistanceBtwCities.WebApi.Controllers
{
    [RoutePrefix("api/editroute")]
    public class RouteEditController : ApiController
    {
        private readonly IRouteEditService _routeEditService;

        public RouteEditController(IRouteEditService routeEditService)
        {
            _routeEditService = routeEditService;
        }

        [HttpPut]
        [Route("distance")]
        public async Task<RouteInfo> UpdateRouteDistance(RouteInfo routeInfo)
        {            
            var info = await _routeEditService.UpdateRouteDistance(routeInfo);

            return info;
        }
    }
}