using System;

namespace Digizuite.Exceptions
{
    public class AssetNotFoundException : Exception
    {
        public AssetNotFoundException(string message) : base(message)
        {
        }

        public AssetNotFoundException()
        {
        }

        public AssetNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}