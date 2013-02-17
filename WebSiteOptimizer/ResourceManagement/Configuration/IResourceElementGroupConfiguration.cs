namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    public interface IResourceElementGroupConfiguration
    {
        string Name { get; }

        bool Minify { get; }

        bool Compress { get; }

        int CacheDuration { get; }

        ResourceType ResourceType { get; }
    }
}