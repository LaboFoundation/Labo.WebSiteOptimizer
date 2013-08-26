using System;
using System.IO;
using System.IO.Compression;

namespace Labo.WebSiteOptimizer.Compression
{
    internal sealed class GzipCompressor : ICompressor
    {
        public byte[] Compress(byte[] content)
        {
            if (content == null) throw new ArgumentNullException("content");

            using (MemoryStream ms = new MemoryStream())
            {
                using (Stream stream = new GZipStream(ms, CompressionMode.Compress))
                {
                    stream.Write(content, 0, content.Length);
                    stream.Close();

                    return ms.ToArray();
                }
            }
        }

        public byte[] Decompress(byte[] content)
        {
            if (content == null) throw new ArgumentNullException("content");

            using (MemoryStream contentStream = new MemoryStream(content))
            {
                using (Stream stream = new GZipStream(contentStream, CompressionMode.Decompress))
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
}