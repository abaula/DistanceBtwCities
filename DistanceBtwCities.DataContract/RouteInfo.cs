using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistanceBtwCities.DataContract
{
    [DataContract]
    public class RouteInfo
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public CityInfo City1 { get; set; }
        [DataMember]
        public CityInfo City2 { get; set; }
        [DataMember]
        public int Distance { get; set; }
    }
}
