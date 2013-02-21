using System;
using System.Web;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.Utility;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public sealed class HttpResponseCompressor : IHttpResponseCompressor
    {
        public void Compress(HttpContextBase context, CompressionType compressionType)
        {
            switch (compressionType)
            {
                case CompressionType.Gzip:
                    context.Response.AppendHeader("Content-Encoding", "gzip");
                    break;
                case CompressionType.Deflate:
                    context.Response.AppendHeader("Content-Encoding", "deflate");
                    break;
            }
        }

        public CompressionType GetRequestCompressionType(HttpContextBase context)
        {
            string acceptEncoding = context.Request.Headers["Accept-Encoding"];
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