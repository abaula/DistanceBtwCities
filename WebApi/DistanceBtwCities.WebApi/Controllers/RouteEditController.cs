using System.Web.Http;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

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
        public RouteInfo UpdateRouteDistance(RouteInfo routeInfo)
        {            
            return _routeEditService.UpdateRouteDistance(routeInfo);
        }
    }
}