using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceBtwCities.WebApi.Caching
{
    class CacheSegmentsComparer
    {
        public bool AreEqual(List<string> cacheSegments, string[] requestSegments)
        {
            // начинаем поиск с конца - в последних сегментах Uri различия найти быстрее, чем в первых.
            var segmentsCount = cacheSegments.Count;

            for (int i = segmentsCount - 1; i >= 0; i--)
            {
                var cacheSegment = cacheSegments[i];

                if (cacheSegment == "*" || cacheSegment == "*/")
                    continue;

                var requestSegment = requestSegments[i];

                if (string.Compare(cacheSegment, requestSegment, StringComparison.InvariantCulture) != 0)
                    return false;

            }

            return true;
        }
    }
}