
namespace DistanceBtwCities.Common.Dtos.Requests
{
    public class RouteSearchRequestCityDto
    {
        public long? CityId { get; set; }
        public int MaxDistance { get; set; }
        public int Offset { get; set; }
        public int Rows { get; set; }
    }
}
