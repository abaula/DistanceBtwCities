using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistanceBtwCities.DatabaseApi.Constants;
using DistanceBtwCities.DatabaseApi.Helpers;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.DatabaseApi.Adapters
{
    static class RouteInfoDbAdapter
    {
        public static RouteInfo GetRouteInfo(this SqlDataReader reader)
        {
            var route = DataContractFactory.CreateRouteInfo();

            route.Id = reader.GetValue<long>(1);

            var city1 = DataContractFactory.CreateCityInfo();
            city1.Id = reader.GetValue<long>(2);
            city1.Name = reader.GetValue<string>(3);
            city1.Fullname = reader.GetValue<string>(4);
            city1.Latitude = (double)reader.GetValue<int>(5) / GeoConstants.GeoFactor;
            city1.Longitude = (double)reader.GetValue<int>(6) / GeoConstants.GeoFactor;

            route.City1 = city1;

            var city2 = DataContractFactory.CreateCityInfo();
            city2.Id = reader.GetValue<long>(7);
            city2.Name = reader.GetValue<string>(8);
            city2.Fullname = reader.GetValue<string>(9);
            city2.Latitude = (double)reader.GetValue<int>(10) / GeoConstants.GeoFactor;
            city2.Longitude = (double)reader.GetValue<int>(11) / GeoConstants.GeoFactor;

            route.City2 = city2;
            route.Distance = reader.GetValue<int>(12);
            
            return route;
        }
    }
}
