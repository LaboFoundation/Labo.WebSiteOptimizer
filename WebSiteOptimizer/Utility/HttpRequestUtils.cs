using System;
using System.Web;
using Labo.WebSiteOptimizer.Compression;

namespace Labo.WebSiteOptimizer.Utility
{
    public static class HttpRequestUtils
    {
        /// <summary>
        /// Gets the type of the request compression.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns></returns>
        public static CompressionType GetRequestCompressionType(HttpRequestBase httpRequest)
        {
            string acceptEncoding = httpRequest.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding))
            {
                return CompressionType.None;
            }

            if (StringUtils.Contains(acceptEncoding, "gzip", StringComparison.OrdinalIgnoreCase))
            {
                return CompressionType.Gzip;
            }

            if (StringUtils.Contains(acceptEncoding, "deflate", StringComparison.OrdinalIgnoreCase))
            {
                return CompressionType.Deflate;
            }

            return CompressionType.None;
        }
    }
}
