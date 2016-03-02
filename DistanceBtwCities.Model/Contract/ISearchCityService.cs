using System.Collections.Generic;
using System.Threading.Tasks;
using DistanceBtwCities.DataContract;

namespace DistanceBtwCities.Model.Contract
{
    public interface ISearchCityService
    {
        Task<IList<CityInfo>> SearchCityAsync(string query);
    }
}