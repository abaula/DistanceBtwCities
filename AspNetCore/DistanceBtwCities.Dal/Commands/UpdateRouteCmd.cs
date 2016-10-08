using DistanceBtwCities.Common.Abstractions.Dal;
using DistanceBtwCities.Common.Connections.Abstractions;
using DistanceBtwCities.Common.Dtos.Requests;
using DistanceBtwCities.Dal.Common;

namespace DistanceBtwCities.Dal.Commands
{
    public class UpdateRouteCmd : DaoBase, ICommand<RouteUpdateDistanceRequestDto>
    {
        public UpdateRouteCmd(IDistanceBtwCitiesConnection connection) : base(connection)
        {
        }

        public void Execute(RouteUpdateDistanceRequestDto cmd)
        {
            var parameters = CreateDynamicParameters();
            parameters.Add("@routeId", cmd.RouteId);
            parameters.Add("@distance", cmd.Distance);

            Execute("dbo.api_UpdateRouteDistance", parameters);
        }
    }
}
