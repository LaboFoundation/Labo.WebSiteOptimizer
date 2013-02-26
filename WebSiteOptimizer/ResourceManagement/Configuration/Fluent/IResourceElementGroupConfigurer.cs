using System;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration.Fluent
{
    public interface IResourceElementGroupConfigurer
    {
        IResourceElementGroupConfigurer Named(string name);

        IResourceElementGroupConfigurer Minify(bool @value);

        IResourceElementGroupConfigurer Compress(bool @value);

        IResourceElementGroupConfigurer CacheDuration(int seconds);
        IResourceElementGroupConfigurer AddResource(Action<IResourceElementConfigurer> registration);
    }
}