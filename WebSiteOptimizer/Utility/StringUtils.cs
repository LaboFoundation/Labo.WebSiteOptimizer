using System;
using System.Globalization;

namespace Labo.WebSiteOptimizer.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Formats the specified string.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string Format(string @string, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, @string, args);
        }

        /// <summary>
        /// Determines whether the [value] [contains] in [the specified target].
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>
        ///   <c>true</c> if the [value] [contains] in [the specified target]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(string target, string value, StringComparison comparisonType)
        {
            return target != null && value != null && target.IndexOf(value, comparisonType) >= 0;
        }
    }
}
