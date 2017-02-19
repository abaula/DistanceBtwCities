using System.Threading.Tasks;
using DistanceBtwCities.Common.Abstractions.Dal;
using DistanceBtwCities.Common.Connections.Abstractions;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Dal.Common;

namespace DistanceBtwCities.Dal.Commands
{
    public class UpdateRouteCmd : DaoBase, ICommand<RouteUpdateInfo>
    {
        public UpdateRouteCmd(IDistanceBtwCitiesContext context) 
            : base(context)
        {
        }

        public async Task Execute(RouteUpdateInfo cmd)
        {
            var parameters = CreateDynamicParameters();
            parameters.Add("@routeId", cmd.Id);
            parameters.Add("@distance", cmd.Distance);
            await Execute("dbo.api_UpdateRouteDistance", parameters)
                .ConfigureAwait(false);
        }
    }
}
