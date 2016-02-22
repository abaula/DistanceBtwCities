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