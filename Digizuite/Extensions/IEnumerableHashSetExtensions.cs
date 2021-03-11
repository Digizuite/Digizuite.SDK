using System.Collections.Generic;

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
    }
}