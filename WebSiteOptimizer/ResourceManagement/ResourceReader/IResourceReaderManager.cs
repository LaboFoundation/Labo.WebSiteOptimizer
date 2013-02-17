namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    public interface IResourceReaderManager
    {
        ResourceInfo ReadResource(ResourceReadOptions resourceConfig);
    }
}