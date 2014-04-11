using System.Web.Mvc.Razor;
using System.Web.Razor.Generator;
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

        public override RazorCodeGenerator DecorateCodeGenerator(RazorCodeGenerator incomingCodeGenerator)
        {
            if (incomingCodeGenerator is CSharpRazorCodeGenerator)
            {
                return new HtmlMinifierMvcCSharpRazorCodeGenerator(incomingCodeGenerator.ClassName,
                    incomingCodeGenerator.RootNamespaceName,
                    incomingCodeGenerator.SourceFileName,
                    incomingCodeGenerator.Host,
                    m_HtmlPageMinifier,
                    m_DebugStatusReader);
            }

            return base.DecorateCodeGenerator(incomingCodeGenerator);
        }
    }
}
