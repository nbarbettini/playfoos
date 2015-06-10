using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core
{
    public static class Extensions
    {
        internal static List<T> DeepClone<T>(this List<T> source)
            where T : IDeepClonable<T>
        {
            return source.ConvertAll(x => x.DeepClone());
        }

        internal static bool SafeSequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null && second == null)
                return true;

            return ((first != null && second != null) &&
                first.SequenceEqual(second));
        }

        // From http://stackoverflow.com/a/22489094/3191599
        internal static IEnumerable<TSource> DistinctBy<TSource, TKey>(
                                this IEnumerable<TSource> source,
                                Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(i => i.First());
        }
    }
}
