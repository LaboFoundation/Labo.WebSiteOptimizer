using System.Web;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public interface IResourceManager
    {
        string RenderJavascriptInclude(HttpContextBase httpContext, string resourceGroupName);

        string RenderCssInclude(HttpContextBase httpContext, string resourceGroupName);
    }
}