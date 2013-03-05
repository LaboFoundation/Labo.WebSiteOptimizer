using System;
using System.Linq;
using System.Text;
using System.Web.Mvc.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Text;
using System.Web.Razor.Tokenizer.Symbols;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;

namespace Labo.WebSiteOptimizer.Mvc4
{
    public sealed class HtmlMinifierMvcWebPageRazorHost : MvcWebPageRazorHost
    {
        private readonly IHtmlPageMinifier m_HtmlPageMinifier;
        private readonly IDebugStatusReader m_DebugStatusReader;

        public HtmlMinifierMvcWebPageRazorHost(IHtmlPageMinifier htmlPageMinifier, IDebugStatusReader debugStatusReader, string virtualPath, string physicalPath)
            : base(virtualPath, physicalPath)
        {
            m_DebugStatusReader = debugStatusReader;
            m_HtmlPageMinifier = htmlPageMinifier;
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
