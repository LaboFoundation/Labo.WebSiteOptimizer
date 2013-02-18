using System;
using System.Globalization;

namespace Labo.WebSiteOptimizer.Utility
{
    public static class ObjectUtils
    {
        public static string ToStringInvariant(object @object)
        {
            return Convert.ToString(@object, CultureInfo.InvariantCulture);
        }
    }
}
