using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;

namespace Labo.WebSiteOptimizer.Compression
{
    internal sealed class DeflateCompressor : ICompressor
    {
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

        public byte[] Decompress(byte[] content)
        {
            using (Stream stream = new DeflateStream(new MemoryStream(content), CompressionMode.Decompress))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    const int size = 4096;
                    byte[] buffer = new byte[size];

                    int count;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            ms.Write(buffer, 0, count);
                        }
                    } while (count > 0);

                    return ms.ToArray();
                }
            }
        }
    }
}