using System;
using System.Collections.Generic;
using System.Globalization;

namespace Labo.WebSiteOptimizer.Compression
{
    public sealed class CompressionFactory : ICompressionFactory
    {
        private static readonly Dictionary<string, Lazy<ICompressor>> s_Dictionary = InitFactory();

        private static Dictionary<string, Lazy<ICompressor>> InitFactory()
        {
            Dictionary<string, Lazy<ICompressor>> dictionary = new Dictionary<string, Lazy<ICompressor>>();
            dictionary.Add(CompressionType.Deflate.ToString(), new Lazy<ICompressor>(() => new DeflateCompressor(), true));
            dictionary.Add(CompressionType.Gzip.ToString(), new Lazy<ICompressor>(() => new GzipCompressor(), true));
            return dictionary;
        }

        public ICompressor CreateCompressor(CompressionType compressionType)
        {
            Lazy<ICompressor> compressor;

            if(s_Dictionary.TryGetValue(compressionType.ToString(), out compressor))
            {
                return compressor.Value;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Unknown Compressor Type: '{0}'", compressionType.ToString()));
        }
    }
}
