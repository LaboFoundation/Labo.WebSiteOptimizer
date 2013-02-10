using System;
using System.Collections.Generic;

namespace Labo.WebSiteOptimizer.Caching
{
    public interface ICacheProvider
    {
        object Get(string key);

        void Set(string key, object data, TimeSpan expiration, List<string> dependentFiles);

        T GetOrAdd<T>(string key, Func<T> funcData, TimeSpan expiration, Func<List<string>> funcDependentFiles);
    }
}
