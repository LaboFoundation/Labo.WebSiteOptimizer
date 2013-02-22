namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    public interface IResourceConfigurationProvider
    {
        ResourceElementGroup GetResourceElementGroup(ResourceType resourceType, string resourceGroupName);
    }
}