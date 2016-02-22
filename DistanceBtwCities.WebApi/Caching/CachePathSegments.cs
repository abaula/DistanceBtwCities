using System.Collections.Generic;
using System.Text;

namespace DistanceBtwCities.WebApi.Caching
{
    public class CachePathSegments
    {
        public CachePathSegments(string relativeUri)
        {
            RelativeUri = relativeUri;
            Segments = new List<string>();
            _createSegments();
        }

        public string RelativeUri { get; set; }
        public List<string> Segments { get; }

        private void _createSegments()
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