namespace Labo.WebSiteOptimizer.ResourceManagement.Transformer
{
    public interface IResourceTransformer
    {
        ResourceReadInfo Transform(ResourceReadInfo resourceReadInfo);
    }
}
