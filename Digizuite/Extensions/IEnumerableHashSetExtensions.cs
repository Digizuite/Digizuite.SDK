using System.Collections.Generic;
using System.Linq;

namespace Digizuite.Extensions
{
    /// <summary>
    ///     These methods are not included in IEnumerableExtensions,
    ///     since it makes the ToHashSet methods ambiguous when used from .NET 5.0
    /// </summary>
    internal static class EnumerableHashSetExtensions
    {
        // For some ungodly reason these aren't defined for netstandard2.0, however they	
        // do exist in both framework 4.7 and core 2.0... 	
        // ms...
        internal static HashSet<T> ToHashSetNetstandard<T>(this IEnumerable<T> enumerable)
        {
            return new HashSet<T>(enumerable);
        }
        
        internal static HashSet<T> ToHashSetNetstandard<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
        {
            return new HashSet<T>(enumerable, comparer);
        }
        
        
        /// <summary>
        ///     A simple reimplementation of the SetEquals method,
        ///     since the default implementation does not use the comparer
        ///     that is specified for the HashSet.
        /// </summary>
        internal static bool ComparerSetEquals<T>(this HashSet<T> a, HashSet<T> b)
        {
            var comparer = a.Comparer;
            return a.Count == b.Count && a.All(e => b.Contains(e, comparer));
        }
    }
}