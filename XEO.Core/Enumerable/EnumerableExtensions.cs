using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEO.Core.Enumerable
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> list, T element)
        {
            foreach (var e in list)
            {
                yield return e;
            }

            yield return element;
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> list, IEnumerable<T> secondList)
        {
            foreach (var e in list)
            {
                yield return e;
            }

            foreach (var e in secondList)
            {
                yield return e;
            }
        }
    }
}
