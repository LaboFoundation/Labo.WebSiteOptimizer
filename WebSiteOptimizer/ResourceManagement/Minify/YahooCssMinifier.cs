using System.Text;
using Yahoo.Yui.Compressor;

namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public sealed class YahooCssMinifier : ICssMinifier
    {
        public byte[] Minify(byte[] content)
        {
            return Encoding.UTF8.GetBytes(Minify(Encoding.UTF8.GetString(content)));
        }

        public string Minify(string content)
        {
            CssCompressor cssCompressor = new CssCompressor
            {
                CompressionType = CompressionType.Standard,
                RemoveComments = true,
                LineBreakPosition = -1
            };
            return cssCompressor.Compress(content);
        }
    }
}
