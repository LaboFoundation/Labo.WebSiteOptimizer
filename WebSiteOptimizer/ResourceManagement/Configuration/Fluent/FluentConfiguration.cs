namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration.Fluent
{
    public static class FluentConfiguration
    {
        public static IInMemoryConfigurer InMemoryConfiguration { get { return new InMemoryConfigurer(new WebResources()); } }
    }
}
