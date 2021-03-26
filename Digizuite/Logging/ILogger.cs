using System;

namespace Digizuite.Logging
{
    public interface ILogger
    {
        void Log(LogLevel level, Exception? exception, string message, params object?[] args);
        bool IsLogLevelEnabled(LogLevel level);
    }

    // ReSharper disable once UnusedTypeParameter
    public interface ILogger<out T> : ILogger
    {
    }
}