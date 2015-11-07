using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    public interface ISearchCityTask
    {
        Task SearchCityAsync(string query);
        List<CityInfo> Cities { get; }
    }
}
