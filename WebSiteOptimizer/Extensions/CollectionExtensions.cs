using Labo.WebSiteOptimizer.Utility;

namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            CollectionUtils.ForEach(collection, action);
        }

        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            CollectionUtils.ForEach(list, action);
        }
    }
}
