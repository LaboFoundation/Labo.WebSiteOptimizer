namespace Labo.WebSiteOptimizer.Compression
{
    public interface ICompressionFactory
    {
        ICompressor CreateCompressor(CompressionType compressionType);
    }
}