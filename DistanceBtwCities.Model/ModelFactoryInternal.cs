using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistanceBtwCities.DataContract;
using DistanceBtwCities.Model.Contract;

namespace DistanceBtwCities.Model
{
    static class ModelFactoryInternal
    {
        public static CityInfo CreateCityInfo()
        {
           return new CityInfo(); 
        }
    }
}
