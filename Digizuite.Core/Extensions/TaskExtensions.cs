using System;
using System.Threading.Tasks;

namespace Digizuite.Extensions
{
    internal static class TaskExtensions
    {
        internal static async Task<T> AndThen<T>(this Task<T> t, Func<T, Task> continuation)
        {
            var result = await t.ConfigureAwait(false);
            await continuation(result).ConfigureAwait(false);
            return result;
        }
    }
}