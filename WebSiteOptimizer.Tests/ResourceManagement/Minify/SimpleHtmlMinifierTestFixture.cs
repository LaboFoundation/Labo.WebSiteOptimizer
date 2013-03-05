using Labo.WebSiteOptimizer.ResourceManagement.Minify;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.Minify
{
    [TestFixture]
    public class SimpleHtmlMinifierTestFixture
    {
        [Test]
        [TestCase("", "")]
        [TestCase("\r\n", "")]
        [TestCase("\r\nSomething", "\nSomething")]
        [TestCase(".</h1>\r\n    <h2>", ".</h1>\n<h2>")]
        [TestCase("\r\n<hgroup class=\"title\">\r\n    <h1>", "\n<hgroup class=\"title\">\n<h1>")]
        [TestCase("\r\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <title>Error</title>\r\n</head>\r\n<body>\r\n    <h2>\r\n        Sorry, an error occurred while processing your request.\r\n    </h2>\r\n</body>\r\n</html>",
                  "\n<!DOCTYPE html>\n<html>\n<head>\n<title>Error</title>\n</head>\n<body>\n<h2>\nSorry, an error occurred while processing your request.\n</h2>\n</body>\n</html>")]
        [TestCase(
            "</h2>\r\n</hgroup>\r\n\r\n<article>\r\n    <p>\r\n        Use this area to provide additional information.\r\n    </p>\r\n\r\n    <p>\r\n        Use this area to provide additional information.\r\n    </p>\r\n\r\n    <p>\r\n        Use this area to provide additional information.\r\n    </p>\r\n</article>\r\n\r\n<aside>\r\n    <h3>Aside Title</h3>\r\n    <p>\r\n        Use this area to provide additional information.\r\n    </p>\r\n    <ul>\r\n        <li>", 
            "</h2>\n</hgroup>\n<article>\n<p>\nUse this area to provide additional information.\n</p>\n<p>\nUse this area to provide additional information.\n</p>\n<p>\nUse this area to provide additional information.\n</p>\n</article>\n<aside>\n<h3>Aside Title</h3>\n<p>\nUse this area to provide additional information.\n</p>\n<ul>\n<li>")]
        [TestCase(
            "</h2>\r\n            </hgroup>\r\n            <p>\r\n                To learn more about ASP.NET MVC visit\r\n                <a href=\"http://asp.net/mvc\" title=\"ASP.NET MVC Website\">http://asp.net/mvc</a>.\r\n                The page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET MVC.\r\n                If you have any questions about ASP.NET MVC visit\r\n                <a href=\"http://forums.asp.net/1146.aspx/1?MVC\" title=\"ASP.NET MVC Forum\">our forums</a>.\r\n            </p>\r\n        </div>\r\n    </section>\r\n",
            "</h2>\n</hgroup>\n<p>\nTo learn more about ASP.NET MVC visit\n<a href=\"http://asp.net/mvc\" title=\"ASP.NET MVC Website\">http://asp.net/mvc</a>.\nThe page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET MVC.\nIf you have any questions about ASP.NET MVC visit\n<a href=\"http://forums.asp.net/1146.aspx/1?MVC\" title=\"ASP.NET MVC Forum\">our forums</a>.\n</p>\n</div>\n</section>\n")]
        [TestCase(
            "\r\n<h3>We suggest the following:</h3>\r\n<ol class=\"round\">\r\n    <li class=\"one\">\r\n        <h5>Getting Started</h5>\r\n        ASP.NET MVC gives you a powerful, patterns-based way to build dynamic websites that\r\n        enables a clean separation of concerns and that gives you full control over markup\r\n        for enjoyable, agile development. ASP.NET MVC includes many features that enable\r\n        fast, TDD-friendly development for creating sophisticated applications that use\r\n        the latest web standards.\r\n        <a href=\"http://go.microsoft.com/fwlink/?LinkId=245151\">Learn more…</a>\r\n    </li>\r\n\r\n    <li class=\"two\">\r\n        <h5>Add NuGet packages and jump-start your coding</h5>\r\n        NuGet makes it easy to install and update free libraries and tools.\r\n        <a href=\"http://go.microsoft.com/fwlink/?LinkId=245153\">Learn more…</a>\r\n    </li>\r\n\r\n    <li class=\"three\">\r\n        <h5>Find Web Hosting</h5>\r\n        You can easily find a web hosting company that offers the right mix of features\r\n        and price for your applications.\r\n        <a href=\"http://go.microsoft.com/fwlink/?LinkId=245157\">Learn more…</a>\r\n    </li>\r\n</ol>\r\n",
            "\n<h3>We suggest the following:</h3>\n<ol class=\"round\">\n<li class=\"one\">\n<h5>Getting Started</h5>\nASP.NET MVC gives you a powerful, patterns-based way to build dynamic websites that\nenables a clean separation of concerns and that gives you full control over markup\nfor enjoyable, agile development. ASP.NET MVC includes many features that enable\nfast, TDD-friendly development for creating sophisticated applications that use\nthe latest web standards.\n<a href=\"http://go.microsoft.com/fwlink/?LinkId=245151\">Learn more…</a>\n</li>\n<li class=\"two\">\n<h5>Add NuGet packages and jump-start your coding</h5>\nNuGet makes it easy to install and update free libraries and tools.\n<a href=\"http://go.microsoft.com/fwlink/?LinkId=245153\">Learn more…</a>\n</li>\n<li class=\"three\">\n<h5>Find Web Hosting</h5>\nYou can easily find a web hosting company that offers the right mix of features\nand price for your applications.\n<a href=\"http://go.microsoft.com/fwlink/?LinkId=245157\">Learn more…</a>\n</li>\n</ol>\n")]
        public void Minify(string content, string expectedMinifiedContent)
        {
            SimpleHtmlMinifier htmlMinifier = new SimpleHtmlMinifier();
            string minifiedHtml = htmlMinifier.Minify(content);

            Assert.AreEqual(expectedMinifiedContent, minifiedHtml);
        }

    }
}
