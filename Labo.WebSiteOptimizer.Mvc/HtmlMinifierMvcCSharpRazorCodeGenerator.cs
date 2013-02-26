using System.Web.Mvc.Razor;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;

namespace Labo.WebSiteOptimizer.Mvc
{
    internal sealed class HtmlMinifierMvcCSharpRazorCodeGenerator : MvcCSharpRazorCodeGenerator
    {
        private readonly IHtmlMinifier m_HtmlMinifier;
        private readonly IDebugStatusReader m_DebugStatusReader;

        public HtmlMinifierMvcCSharpRazorCodeGenerator(string className, string rootNamespaceName, string sourceFileName, 
            RazorEngineHost host, IHtmlMinifier htmlMinifier, IDebugStatusReader debugStatusReader)
            : base(className, rootNamespaceName, sourceFileName, host)
        {
            m_HtmlMinifier = htmlMinifier;
            m_DebugStatusReader = debugStatusReader;
        }

        public override void VisitSpan(Span span)
        {
            if (span.Kind == SpanKind.Markup && !m_DebugStatusReader.IsDebuggingEnabled())
            {
                string content = span.Content;
                span.Content = m_HtmlMinifier.Minify(content, true, true);
            }
       
            base.VisitSpan(span);
        }
    }
}
