using System.Web.Mvc;

namespace Labo.WebSiteOptimizer.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static MvcControlsFactory WebSiteOptimizer(this HtmlHelper htmlHelper)
        {
            return new MvcControlsFactory(htmlHelper);
        }
    }
}
