using System.Collections.Generic;
using DistanceBtwCities.Common.Abstractions.Domain;
using DistanceBtwCities.Common.Dtos;
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
        public IEnumerable<CityInfo> SearchCity(string query)
        {
            return _citiesService.SearchCity(query);
        }
    }
}
