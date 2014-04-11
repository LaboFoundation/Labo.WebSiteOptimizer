using System.Web.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Text;
using System.Web.Razor.Tokenizer.Symbols;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;

namespace Labo.WebSiteOptimizer.Mvc4
{
    internal sealed class HtmlMinifierMvcCSharpRazorCodeGenerator : CSharpRazorCodeGenerator
    {
        private sealed class MarkupSymbol : ISymbol
        {
            private SourceLocation m_Start = SourceLocation.Zero;

            public void ChangeStart(SourceLocation newStart)
            {
                m_Start = newStart;
            }

            public string Content { get; set; }

            public void OffsetStart(SourceLocation documentStart)
            {
                m_Start = documentStart;
            }

            public SourceLocation Start
            {
                get { return m_Start; }
            }
        }

        private readonly IHtmlPageMinifier m_HtmlPageMinifier;
        private readonly IDebugStatusReader m_DebugStatusReader;

        public HtmlMinifierMvcCSharpRazorCodeGenerator(string className, string rootNamespaceName, string sourceFileName, RazorEngineHost host, IHtmlPageMinifier htmlPageMinifier, IDebugStatusReader debugStatusReader)
            : base(className, rootNamespaceName, sourceFileName, host)
        {
            m_HtmlPageMinifier = htmlPageMinifier;
            m_DebugStatusReader = debugStatusReader;
        }

        public override void VisitSpan(Span span)
        {
            if (span.Kind == SpanKind.Markup && !m_DebugStatusReader.IsDebuggingEnabled())
            {
                string content = m_HtmlPageMinifier.Minify(span.Content, true, true);

                SpanBuilder builder = new SpanBuilder { CodeGenerator = span.CodeGenerator, EditHandler = span.EditHandler, Kind = span.Kind, Start = span.Start };
                MarkupSymbol symbol = new MarkupSymbol { Content = content };
                builder.Accept(symbol);
                span.ReplaceWith(builder);
            }
       
            base.VisitSpan(span);
        }
    }
}