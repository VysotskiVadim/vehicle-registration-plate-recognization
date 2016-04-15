using System;
using System.Collections.Generic;
using System.Linq;

namespace vrpr.Core
{
    public static class Helper
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            var forEach = collection as T[] ?? collection.ToArray();
            foreach (var item in forEach)
            {
                action(item);
            }
            return forEach;
        } 
    }
}
