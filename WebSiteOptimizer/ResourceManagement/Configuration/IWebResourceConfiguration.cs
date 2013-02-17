namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    public interface IWebResourceConfiguration
    {
        ResourceElementGroup GetResourceElementGroup(ResourceType resourceType, string resourceGroupName);
    }
}