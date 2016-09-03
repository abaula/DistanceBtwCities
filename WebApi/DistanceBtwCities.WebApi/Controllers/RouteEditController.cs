using System.Threading.Tasks;
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
        public async Task<RouteInfo> UpdateRouteDistance(RouteInfo routeInfo)
        {            
            var info = await Task.Run(() => _routeEditService.UpdateRouteDistance(routeInfo));

            return info;
        }
    }
}