using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DistanceBtwCities.WebApi.Caching
{
    public class CachePathSegments
    {
        public string RelativeUri { get; set; }
        public List<string> Segments { get; private set; }

        public CachePathSegments(string relativeUri)
        {
            RelativeUri = relativeUri;
            Segments = new List<string>();
            _createSegments();
        }

        void _createSegments()
        {
            var charArray = RelativeUri.ToCharArray();
            var sb = new StringBuilder();

            foreach (var symbol in charArray)
            {
                sb.Append(symbol);

                if (symbol == '/')
                {
                    Segments.Add(sb.ToString());
                    sb.Clear();
                }
            }

            Segments.Add(sb.ToString());
        }

    }
}