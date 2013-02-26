namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration.Fluent
{
    public sealed class InMemoryConfigurationProvider : IResourceConfigurationProvider
    {
        private readonly WebResources m_WebResources;

        public InMemoryConfigurationProvider(WebResources webResources)
        {
            m_WebResources = webResources;
        }

        public ResourceElementGroup GetResourceElementGroup(ResourceType resourceType, string resourceGroupName)
        {
            return m_WebResources.GetResourceElementGroup(resourceType, resourceGroupName);
        }
    }
}
