using System.Web;
using Labo.WebSiteOptimizer.Compression;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public interface IHttpResponseCompressor
    {
        void Compress(HttpContextBase context, CompressionType compressionType);

        CompressionType GetRequestCompressionType(HttpContextBase context);
    }
}
