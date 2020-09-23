using System;

namespace Digizuite.Exceptions
{
    public class ProductVersionNotFoundException : Exception
    {
        public ProductVersionNotFoundException()
        {
        }

        public ProductVersionNotFoundException(string message) : base(message)
        {
        }

        public ProductVersionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
