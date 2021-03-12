using System;
using Digizuite.Models;

namespace Digizuite
{
    public interface ILogger
    {
        void Log(LogLevel level, Exception? exception, string message, params object[] args);
    }

    // ReSharper disable once UnusedTypeParameter
    public interface ILogger<out T> : ILogger
    {
    }
}