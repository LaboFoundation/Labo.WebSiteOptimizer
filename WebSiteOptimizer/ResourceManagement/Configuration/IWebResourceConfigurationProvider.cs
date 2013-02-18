namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    public interface IWebResourceConfigurationProvider
    {
        ResourceElementGroup GetResourceElementGroup(ResourceType resourceType, string resourceGroupName);
    }
}