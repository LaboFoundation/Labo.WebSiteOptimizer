using Labo.WebSiteOptimizer.ResourceManagement.Minify;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.Minify
{
    [TestFixture]
    public class YahooJsMinifierTestFixture
    {
        private const string JS = @"var StringBuilder = (function () {
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
        public void Minify()
        {
            
            YahooJsMinifier yahooJsMinifier = new YahooJsMinifier();
            const string expectedMinifiedJs = "var StringBuilder=(function(){function StringBuilder(){this.strings=[]}StringBuilder.prototype.append=function(text){this.strings.push(text)};StringBuilder.prototype.toString=function(){return this.strings.join(\"\")};return StringBuilder})();";
            
            Assert.AreEqual(expectedMinifiedJs, yahooJsMinifier.Minify(JS));
        }

        [Test]
        public void Minify_WithEnableOptimizationsAndObfuscateTrue()
        {
            YahooJsMinifier yahooJsMinifier = new YahooJsMinifier();
            const string expectedMinifiedJs = "var StringBuilder=(function(){function a(){this.strings=[]}a.prototype.append=function(b){this.strings.push(b)};a.prototype.toString=function(){return this.strings.join(\"\")};return a})();";

            Assert.AreEqual(expectedMinifiedJs, yahooJsMinifier.Minify(JS, true, true));
        }
    }
}
