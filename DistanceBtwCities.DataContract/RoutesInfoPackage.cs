using System.Collections.Generic;
using System.Runtime.Serialization;

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