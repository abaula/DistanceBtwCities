using DistanceBtwCities.Common.Abstractions.Domain;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Common.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DistanceBtwCities.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    public class RoutesController : Controller
    {
        private readonly IRoutesService _routesService;

        public RoutesController(IRoutesService routesService)
        {
            _routesService = routesService;
        }

        [HttpGet]
        public RoutesInfoPackage SearchRouteForQuery([FromQuery]RouteSearchRequestDto request)
        {
            return _routesService.SearchRoute(request);
        }

        [HttpPut]
        public RouteInfo UpdateRouteDistance([FromBody]RouteInfo request)
        {
            return _routesService.UpdateRouteDistance(request);
        }
    }
}
