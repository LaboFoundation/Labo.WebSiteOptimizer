using System;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration.Fluent
{
    public interface IInMemoryConfigurer
    {
        IInMemoryConfigurer AddCssGroup(string name, Action<IResourceElementGroupConfigurer> registration);

        IInMemoryConfigurer AddJsGroup(string name, Action<IResourceElementGroupConfigurer> registration);

        void Configure();
    }
}