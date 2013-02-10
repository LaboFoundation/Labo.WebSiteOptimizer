using Yahoo.Yui.Compressor;

namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public sealed class YahooCssMinifier : ICssMinifier
    {
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
