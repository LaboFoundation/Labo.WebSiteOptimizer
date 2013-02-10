namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    internal interface IResourceReaderManager
    {
        ResourceInfo ReadResource(ResourceReadOptions resourceConfig);
    }
}