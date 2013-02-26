using System.Web.Mvc.Razor;
using System.Web.Razor.Generator;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;

namespace Labo.WebSiteOptimizer.Mvc
{
    internal sealed class HtmlMinifierMvcWebPageRazorHost : MvcWebPageRazorHost
    {
        private readonly IHtmlMinifier m_HtmlMinifier;
        private readonly IDebugStatusReader m_DebugStatusReader;

        public HtmlMinifierMvcWebPageRazorHost(string virtualPath, string physicalPath, IHtmlMinifier htmlMinifier, IDebugStatusReader debugStatusReader)
            : base(virtualPath, physicalPath)
        {
            m_HtmlMinifier = htmlMinifier;
            m_DebugStatusReader = debugStatusReader;
        }

        public override RazorCodeGenerator DecorateCodeGenerator(RazorCodeGenerator incomingCodeGenerator)
        {
            if (incomingCodeGenerator is CSharpRazorCodeGenerator)
            {
                return new HtmlMinifierMvcCSharpRazorCodeGenerator(
                    incomingCodeGenerator.ClassName,
                    incomingCodeGenerator.RootNamespaceName,
                    incomingCodeGenerator.SourceFileName,
                    incomingCodeGenerator.Host,
                    m_HtmlMinifier,
                    m_DebugStatusReader);
            }

            return base.DecorateCodeGenerator(incomingCodeGenerator);
        }
    }
}