using System.Threading.Tasks;
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
        public async Task<RoutesInfoPackage> SearchRouteForQuery([FromQuery]RouteSearchRequestDto request)
        {
            return await Task.Run(() => _routesService.SearchRoute(request));
        }

        [HttpPut]
        public async Task UpdateRouteDistance([FromBody]RouteUpdateDistanceRequestDto request)
        {
            await Task.Run(() => _routesService.UpdateRouteDistance(request));
        }
    }
}
