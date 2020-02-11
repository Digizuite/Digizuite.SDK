using System;

namespace Digizuite.Exceptions
{
    public class ResponsibilitiesNotAcceptedException : Exception
    {
        public ResponsibilitiesNotAcceptedException(string message) : base(message)
        {
        }

        public ResponsibilitiesNotAcceptedException()
        {
        }

        public ResponsibilitiesNotAcceptedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}