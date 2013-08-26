using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace Labo.WebSiteOptimizer.Caching
{
    internal sealed class HttpRuntimeCacheProvider : ICacheProvider
    {
        private static readonly object s_LockObject = new object();

        public object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        public void Set(string key, object data, TimeSpan expiration, List<string> dependentFiles)
        {
            if (dependentFiles == null) throw new ArgumentNullException("dependentFiles");

            HttpRuntime.Cache.Insert(key, data, new CacheDependency(dependentFiles.ToArray()), Cache.NoAbsoluteExpiration, expiration, CacheItemPriority.NotRemovable, (s, value, reason) => {});
        }

        public T GetOrAdd<T>(string key, Func<T> funcData, TimeSpan expiration, Func<List<string>> funcDependentFiles)
        {
            if (funcData == null) throw new ArgumentNullException("funcData");
            if (funcDependentFiles == null) throw new ArgumentNullException("funcDependentFiles");

            object data = Get(key);
            if (data == null)
            {
                lock (s_LockObject)
                {
                    T value = funcData();
                    Set(key, value, expiration, funcDependentFiles());
                    return value;
                }
            }
            return (T)data;
        }
    }
}