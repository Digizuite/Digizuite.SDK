using System;

namespace Digizuite.Helpers
{
    public static class Guard
    {
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        public static void AgainstNull<T>(T? item, string argumentName)
        {
            if (item == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}