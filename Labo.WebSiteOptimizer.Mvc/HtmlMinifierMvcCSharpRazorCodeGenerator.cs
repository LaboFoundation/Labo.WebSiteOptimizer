using System.Web.Mvc.Razor;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;

namespace Labo.WebSiteOptimizer.Mvc
{
    internal sealed class HtmlMinifierMvcCSharpRazorCodeGenerator : MvcCSharpRazorCodeGenerator
    {
        private readonly IHtmlPageMinifier m_HtmPagelMinifier;
        private readonly IDebugStatusReader m_DebugStatusReader;

        public HtmlMinifierMvcCSharpRazorCodeGenerator(string className, string rootNamespaceName, string sourceFileName,
            RazorEngineHost host, IHtmlPageMinifier htmlPageMinifier, IDebugStatusReader debugStatusReader)
            : base(className, rootNamespaceName, sourceFileName, host)
        {
            m_HtmPagelMinifier = htmlPageMinifier;
            m_DebugStatusReader = debugStatusReader;
        }

        public override void VisitSpan(Span span)
        {
            if (span.Kind == SpanKind.Markup && !m_DebugStatusReader.IsDebuggingEnabled())
            {
                string content = span.Content;
                span.Content = m_HtmPagelMinifier.Minify(content, true, true);
            }
       
            base.VisitSpan(span);
        }
    }
}
