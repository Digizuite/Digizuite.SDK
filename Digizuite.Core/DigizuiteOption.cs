using System;

namespace Digizuite
{
    [Flags]
    public enum DigizuiteOption
    {
        Nothing = 0,
        SkipCache = 1
    }

    internal static class DigizuiteOptionExtensions
    {
        public static bool HasFlag(this DigizuiteOption value, DigizuiteOption flag)
        {
            return (value & flag) != 0;
        }
    }
}