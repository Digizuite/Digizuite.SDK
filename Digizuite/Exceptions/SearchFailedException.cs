using System;

namespace Digizuite.Exceptions
{
    /// <summary>
    /// Thrown whenever a search fails
    /// </summary>
    public class SearchFailedException : Exception
    {
        public SearchFailedException(string message, Exception? innerException) : base(message, innerException)
        {
        }

        public SearchFailedException(string message) : base(message)
        {
        }

        public SearchFailedException()
        {
        }
    }
}