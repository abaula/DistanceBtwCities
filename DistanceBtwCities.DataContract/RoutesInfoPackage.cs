using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistanceBtwCities.DataContract
{
    [DataContract]
    public class RoutesInfoPackage
    {
        public RoutesInfoPackage()
        {
            Routes = new List<RouteInfo>();
        }

        [DataMember]
        public List<RouteInfo> Routes { get; private set; }

        [DataMember]
        public int AllFoundRoutesCount { get; set; } 
    }
}
