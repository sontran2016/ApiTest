using System.Collections.Generic;
using System.Linq;

namespace Common.Helpers
{
    public static class DictionaryHelper
    {
        /// <summary>
        /// transfer dictionary to dictionary
        /// </summary>
        public static Dictionary<T1, T2> AddToDictionary<T1, T2>(this Dictionary<T1, T2> source,
            Dictionary<T1, T2> destination)
        {
            var first = destination.Keys.FirstOrDefault();
            if (first != null && !source.ContainsKey(first))
            {
                source.Add(destination.FirstOrDefault().Key, destination.FirstOrDefault().Value);
            }
            return source;
        }

    }
}
