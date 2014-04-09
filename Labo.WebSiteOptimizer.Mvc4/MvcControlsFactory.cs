using System.Web.Mvc;
using Labo.WebSiteOptimizer.ResourceManagement;

namespace Labo.WebSiteOptimizer.Mvc4
{
    public sealed class MvcControlsFactory
    {
        private readonly HtmlHelper m_HtmlHelper;

        public MvcControlsFactory(HtmlHelper htmlHelper)
        {
            m_HtmlHelper = htmlHelper;
        }

        public MvcHtmlString CssInclude(string cssResourceGroupName)
        {
            return new MvcHtmlString(ResourceManagerRuntime.ResourceManager.RenderCssInclude(m_HtmlHelper.ViewContext.HttpContext, cssResourceGroupName));
        }

        public MvcHtmlString JsInclude(string jsResourceGroupName)
        {
            return new MvcHtmlString(ResourceManagerRuntime.ResourceManager.RenderJavascriptInclude(m_HtmlHelper.ViewContext.HttpContext, jsResourceGroupName));
        }
    }
}
