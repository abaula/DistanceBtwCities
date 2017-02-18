using System.Threading.Tasks;
using DistanceBtwCities.Common.Abstractions.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DistanceBtwCities.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        private readonly ICitiesService _citiesService;

        public CitiesController(ICitiesService citiesService)
        {
            _citiesService = citiesService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchCity(string query)
        {
            var data = await _citiesService.SearchCity(query);
            return Json(data);
        }
    }
}
