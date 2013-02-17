using System.Web;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public interface IResourceProcessor
    {
        ProcessedResourceGroupInfo ProcessResource(HttpContextBase httpContext, ResourceElementGroup resourceElementGroup);
    }
}