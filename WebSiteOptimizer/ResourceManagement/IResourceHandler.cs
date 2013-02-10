using System.Web;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public interface IResourceHandler
    {
        ResourceGroupInfo ProcessResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName);

        void HandleResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName);
    }
}