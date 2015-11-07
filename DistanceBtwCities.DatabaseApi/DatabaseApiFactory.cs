using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceBtwCities.DatabaseApi
{
    public static class DatabaseApiFactory
    {
        public static DbProcedures CreateDbProcedures(string connectionString)
        {
            return new DbProcedures(connectionString);
        }

    }
}
