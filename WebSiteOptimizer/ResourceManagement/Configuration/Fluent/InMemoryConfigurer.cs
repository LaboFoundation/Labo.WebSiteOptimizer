using System;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration.Fluent
{
    internal sealed class InMemoryConfigurer : IInMemoryConfigurer
    {
        private readonly WebResources m_WebResources;

        public InMemoryConfigurer(WebResources webResources)
        {
            m_WebResources = webResources;
        }

        public IInMemoryConfigurer AddCssGroup(string name, Action<IResourceElementGroupConfigurer> registration)
        {
            return AddResourceGroup(name, registration, ResourceType.Css);
        }

        public IInMemoryConfigurer AddJsGroup(string name, Action<IResourceElementGroupConfigurer> registration)
        {
            return AddResourceGroup(name, registration, ResourceType.Js);
        }

        private IInMemoryConfigurer AddResourceGroup(string name, Action<IResourceElementGroupConfigurer> registration, ResourceType resourceType)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            ResourceElementGroup resourceElementGroup = new ResourceElementGroup {ResourceType = resourceType};
            ResourceElementGroupConfigurer resourceElementGroupConfigurer = new ResourceElementGroupConfigurer(resourceElementGroup);
            registration(resourceElementGroupConfigurer);

            m_WebResources.CssResources.ResourceGroups.Add(resourceElementGroup);
            return this;
        }


        public void Configure()
        {
            ResourceManagerRuntime.SetResourceConfigurationProvider(new InMemoryConfigurationProvider(m_WebResources));
        }
    }
}
