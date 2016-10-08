namespace DistanceBtwCities.Dal.Common
{
    /// <summary>
    /// Вспомогательные константы для расчёта географических координат.
    /// </summary>
    public static class GeoConstants
    {
        /// <summary>
        /// Используется для преобразования широты и долготы из double в int и обратно.
        /// В базе данных координаты храняться в виде целых чисел, на клиенте координаты - числа с плавающей точкой.
        /// </summary>
        public const long GeoFactor = 10000000;
    }
}