using System.Net.Http;
using System.Text;
using System.Web.Http;
using DistanceBtwCities.WebApi.Models;

namespace DistanceBtwCities.WebApi.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            var htmlContent = CachedPageProvider.GetHtmlPageContent(@"/HtmlPages/index.html");

            return new HttpResponseMessage
            {
                Content = new StringContent(htmlContent, Encoding.UTF8, "text/html")
            };
        }
    }
}
