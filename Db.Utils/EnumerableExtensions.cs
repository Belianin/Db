using System;
using System.Collections.Generic;

namespace Db.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var e in enumerable)
            {
                action(e);
                yield return e;
            }
        }
    }
}