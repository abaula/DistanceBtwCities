using System.Data.SqlClient;
using DistanceBtwCities.Dal.Constants;
using DistanceBtwCities.Dal.Helpers;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Dal.Adapters
{
    internal static class CityInfoDbAdapter
    {
        public static CityInfo GetCityInfo(SqlDataReader reader)
        {
            var city = DataContractFactory.CreateCityInfo();

            city.Id = reader.GetValue<long>(0);
            city.Latitude = (double) reader.GetValue<int>(1)/GeoConstants.GeoFactor;
            city.Longitude = (double) reader.GetValue<int>(2)/GeoConstants.GeoFactor;
            city.Name = reader.GetValue<string>(3);
            city.District = reader.GetValue<string>(4);
            city.Region = reader.GetValue<string>(5);
            city.Suffix = reader.GetValue<string>(6);
            city.CladrCode = reader.GetValue<string>(7);
            city.PostCode = reader.GetValue<string>(8);
            city.Fullname = reader.GetValue<string>(9);

            return city;
        }
    }
}