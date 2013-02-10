using System.Text;
using Labo.WebSiteOptimizer.Compression;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.Compression
{
    [TestFixture]
    public class DeflateCompressorTestFixture
    {
        private const string CONTENT = @"var StringBuilder = (function () {
        // constructor
        function StringBuilder() {
            this.strings = [];
        }
        /* 
            Appends text
        */
        StringBuilder.prototype.append = function (text) {
            this.strings.push(text);
        };
        StringBuilder.prototype.toString = function () {
            return this.strings.join('');
        };
        return StringBuilder;
    })();";

        [Test]
        public void CompressDecompress()
        {
            DeflateCompressor gzipCompressor = new DeflateCompressor();
            byte[] compressedContent = gzipCompressor.Compress(Encoding.UTF8.GetBytes(CONTENT));
            Assert.AreEqual(CONTENT, Encoding.UTF8.GetString(gzipCompressor.Decompress(compressedContent)));
        }
    }
}