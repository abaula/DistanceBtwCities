namespace DistanceBtwCities.DataContract
{
    public static class DataContractFactory
    {
        public static CityInfo CreateCityInfo()
        {
            return new CityInfo();
        }

        public static RouteInfo CreateRouteInfo()
        {
            return new RouteInfo();
        }

        public static RoutesInfoPackage CreateRoutesInfoPackage()
        {
            return new RoutesInfoPackage();
        }
    }
}