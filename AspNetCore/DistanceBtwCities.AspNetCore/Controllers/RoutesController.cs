using System.Threading.Tasks;
using DistanceBtwCities.Common.Abstractions.Domain;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Common.Dtos.Requests;
using DistanceBtwCities.Common.Helpers;
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
        public async Task<IActionResult> SearchRouteForQuery([FromQuery]RouteSearchRequestDto request)
        {
            var data = await _routesService.SearchRoute(request);
            return Json(data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRouteDistance([FromBody]RouteUpdateInfo request)
        {
            await _routesService.UpdateRouteDistance(request);
            return Json(EmptyObjectHelper.Empty);
        }
    }
}
