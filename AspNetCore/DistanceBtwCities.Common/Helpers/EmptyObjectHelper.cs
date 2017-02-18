
namespace DistanceBtwCities.Common.Helpers
{
    public static class EmptyObjectHelper
    {
        static EmptyObjectHelper()
        {
            Empty = new object();
        }

        public static object Empty { get; }
    }
}
