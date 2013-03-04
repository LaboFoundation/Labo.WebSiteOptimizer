using System.Web.Mvc.Razor;
using System.Web.Razor.Parser;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;

namespace Labo.WebSiteOptimizer.Mvc4
{
    public sealed class HtmlMinifierMvcWebPageRazorHost : MvcWebPageRazorHost
    {
        private readonly IHtmlMinifier m_HtmlMinifier;
        private readonly IDebugStatusReader m_DebugStatusReader;

        public HtmlMinifierMvcWebPageRazorHost(IHtmlMinifier htmlMinifier, IDebugStatusReader debugStatusReader, string virtualPath, string physicalPath)
            : base(virtualPath, physicalPath)
        {
            m_DebugStatusReader = debugStatusReader;
            m_HtmlMinifier = htmlMinifier;
        }

        public override ParserBase DecorateMarkupParser(ParserBase incomingMarkupParser)
        {
            ParserBase parser = base.DecorateMarkupParser(incomingMarkupParser);
            if (!(parser is HtmlMarkupParser) || m_DebugStatusReader.IsDebuggingEnabled())
            {
                return parser;
            }
            //TODO: Minify HTML
            return null;
        }
    }
}
