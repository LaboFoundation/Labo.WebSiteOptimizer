using System.Web;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public interface IResourceManager
    {
        string RenderCssInclude(HttpContextBase httpContext, string resourceGroupName);

        string RenderJavascriptInclude(HttpContextBase httpContext, string resourceGroupName);
    }
}