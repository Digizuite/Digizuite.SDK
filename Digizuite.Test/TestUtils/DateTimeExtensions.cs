using System;

namespace Digizuite.Test.TestUtils
{
    public static class DateTimeExtensions
    {
        public static DateTime Truncate(this DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.FromSeconds(1).Ticks));
        }
    }
}