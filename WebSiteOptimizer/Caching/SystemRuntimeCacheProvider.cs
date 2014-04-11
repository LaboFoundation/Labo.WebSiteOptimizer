using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Labo.WebSiteOptimizer.Caching
{
    public sealed class SystemRuntimeCacheProvider : ICacheProvider
    {
        private static readonly object s_LockObject = new object();

        public object Get(string key)
        {
            return MemoryCache.Default.Get(key);
        }

        public void Set(string key, object data, TimeSpan expiration, List<string> dependentFiles)
        {
            MemoryCache.Default.Add(key, data, GetCacheItemPolicy(expiration, dependentFiles));
        }

        private static CacheItemPolicy GetCacheItemPolicy(TimeSpan expiration, IList<string> fileDependencies = null)
        {
            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                Priority = CacheItemPriority.NotRemovable,
                SlidingExpiration = expiration
            };

            if (fileDependencies != null)
            {
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(fileDependencies));
            }

            return policy;
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