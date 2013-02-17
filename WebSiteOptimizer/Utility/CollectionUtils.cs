using System;
using System.Collections.Generic;

namespace Labo.WebSiteOptimizer.Utility
{
    public static class CollectionUtils
    {
        public static void ForEach<T>(IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action(item);
            }
        }

        public static void ForEach<T>(IList<T> list, Action<T> action)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                action(item);
            }
        }
    }
}
