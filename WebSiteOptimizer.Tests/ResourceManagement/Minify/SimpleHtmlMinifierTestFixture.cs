using Labo.WebSiteOptimizer.ResourceManagement.Minify;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.Minify
{
    [TestFixture]
    public class SimpleHtmlMinifierTestFixture
    {
        [Test]
        public void Minify()
        {
            SimpleHtmlMinifier htmlMinifier = new SimpleHtmlMinifier(null, null);
            string minifiedHtml = htmlMinifier.Minify(@"<table>
                                                        <tr>
                                                            <td>Name</td>
                                                            <td>Value</td>
                                                        </tr>
                                                      </table>");
            Assert.AreEqual("<table><tr><td>Name</td><td>Value</td></tr></table>", minifiedHtml);
        }

    }
}
