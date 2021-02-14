using System;
using System.Collections.Generic;
using System.Text;

namespace Zed3.Extensions
{
    public static class IReadOnlyListExtensions
    {
        public static ushort? FindIndex<T>(this IReadOnlyList<T> array, T item)
            where T : IEquatable<T>
        {
            for (var i = (ushort)0; i < array.Count; i++)
                if (array[i].Equals(item))
                    return i;

            return null;
        }
    }
}
