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
    }
}
