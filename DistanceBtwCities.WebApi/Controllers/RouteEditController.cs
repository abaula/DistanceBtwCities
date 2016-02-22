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
        [HttpPut]
        [Route("distance")]
        public async Task<RouteInfo> UpdateRouteDistance(RouteInfo routeInfo)
        {
            var routeEditTask = _getRouteEditTask();

            var result = await routeEditTask.UpdateRouteDistance(routeInfo);

            return result;
        }

        private IRouteEditTask _getRouteEditTask()
        {
            var factory = ModelFactory.CreateInstance(Settings.Default.DbConnectionString);
            var routeEditTask = factory.CreateRouteEditTask();

            return routeEditTask;
        }
    }
}