namespace Labo.WebSiteOptimizer.Compression
{
    public interface ICompressor
    {
        byte[] Compress(byte[] content);

        byte[] Decompress(byte[] content);
    }
}