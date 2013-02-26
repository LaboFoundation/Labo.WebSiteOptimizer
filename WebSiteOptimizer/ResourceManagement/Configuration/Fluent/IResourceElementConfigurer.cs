namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration.Fluent
{
    public interface IResourceElementConfigurer
    {
        IResourceElementConfigurer Minify(bool @value);

        IResourceElementConfigurer EmbeddedResource(string fileName);

        IResourceElementConfigurer ResourceFile(string fileName);
    }
}