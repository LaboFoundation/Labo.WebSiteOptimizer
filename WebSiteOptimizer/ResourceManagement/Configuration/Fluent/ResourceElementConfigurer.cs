namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration.Fluent
{
    internal sealed class ResourceElementConfigurer : IResourceElementConfigurer
    {
        private readonly ResourceElement m_ResourceElement;

        public ResourceElementConfigurer(ResourceElement resourceElement)
        {
            m_ResourceElement = resourceElement;
        }

        public IResourceElementConfigurer Minify(bool @value)
        {
            m_ResourceElement.Minify = @value;
            return this;
        }

        public IResourceElementConfigurer EmbeddedResource(string fileName)
        {
            m_ResourceElement.IsEmbeddedResource = true;
            m_ResourceElement.FileName = fileName;
            return this;
        }

        public IResourceElementConfigurer HttpResource(string fileName)
        {
            m_ResourceElement.IsHttpResource = true;
            m_ResourceElement.FileName = fileName;
            return this;
        }

        public IResourceElementConfigurer ResourceFile(string fileName)
        {
            m_ResourceElement.FileName = fileName;
            return this;
        }
    }
}