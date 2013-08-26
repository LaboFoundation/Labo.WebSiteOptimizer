using System;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration.Fluent
{
    internal sealed class ResourceElementGroupConfigurer : IResourceElementGroupConfigurer
    {
        private readonly ResourceElementGroup m_ResourceElementGroup;

        public ResourceElementGroupConfigurer(ResourceElementGroup resourceElementGroup)
        {
            m_ResourceElementGroup = resourceElementGroup;
        }

        public IResourceElementGroupConfigurer Named(string name)
        {
            m_ResourceElementGroup.Name = name;
            return this;
        }

        public IResourceElementGroupConfigurer Minify(bool @value)
        {
            m_ResourceElementGroup.Minify = @value;
            return this;
        }

        public IResourceElementGroupConfigurer Compress(bool @value)
        {
            m_ResourceElementGroup.Compress = @value;
            return this;
        }

        public IResourceElementGroupConfigurer CacheDuration(int seconds)
        {
            m_ResourceElementGroup.CacheDuration = seconds;
            return this;
        }

        public IResourceElementGroupConfigurer AddResource(Action<IResourceElementConfigurer> registration)
        {
            if (registration == null) throw new ArgumentNullException("registration");

            ResourceElement resourceElement = new ResourceElement();
            ResourceElementConfigurer resourceElementConfigurer = new ResourceElementConfigurer(resourceElement);
            registration(resourceElementConfigurer);
            return this;
        }
    }
}