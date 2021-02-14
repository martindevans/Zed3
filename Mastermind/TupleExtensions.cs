using System.Collections.Generic;

namespace Mastermind
{
    internal static class TupleExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this (T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
        }
    }
}
