using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;

namespace Labo.WebSiteOptimizer.Compression
{
    internal sealed class DeflateCompressor : ICompressor
    {
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public byte[] Compress(byte[] content)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Stream stream = new DeflateStream(ms, CompressionMode.Compress))
                {
                    stream.Write(content, 0, content.Length);
                    stream.Close();

                    return ms.ToArray();
                }
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public byte[] Decompress(byte[] content)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Stream stream = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    stream.Write(content, 0, content.Length);
                    stream.Close();

                    return ms.ToArray();
                }
            }
        }
    }
}