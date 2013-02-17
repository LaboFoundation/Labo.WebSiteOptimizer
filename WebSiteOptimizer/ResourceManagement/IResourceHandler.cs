using System.Web;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public interface IResourceHandler
    {
        void HandleResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName);
    }
}