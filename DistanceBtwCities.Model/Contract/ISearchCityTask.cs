using System.Collections.Generic;
using System.Threading.Tasks;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    public interface ISearchCityTask
    {
        List<CityInfo> Cities { get; }
        Task SearchCityAsync(string query);
    }
}