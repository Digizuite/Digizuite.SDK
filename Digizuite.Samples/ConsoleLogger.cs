using System;
using Digizuite.Models;

namespace Digizuite.Samples
{
    internal class ConsoleLogger : ILogger
    {
        public void LogTrace(string message)
        {
            Console.WriteLine(message);
        }

        public void LogTrace(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogTrace(Exception exception, string message)
        {
            Console.WriteLine(message);
        }

        public void LogTrace(Exception exception, string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogDebug(string message)
        {
            Console.WriteLine(message);
        }

        public void LogDebug(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogDebug(Exception exception, string message)
        {
            Console.WriteLine(message);
        }

        public void LogDebug(Exception exception, string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogInformation(string message)
        {
            Console.WriteLine(message);
        }

        public void LogInformation(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogInformation(Exception exception, string message)
        {
            Console.WriteLine(message);
        }

        public void LogInformation(Exception exception, string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogWarning(string message)
        {
            Console.WriteLine(message);
        }

        public void LogWarning(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogWarning(Exception exception, string message)
        {
            Console.WriteLine(message);
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogError(string message)
        {
            Console.WriteLine(message);
        }

        public void LogError(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogError(Exception exception, string message)
        {
            Console.WriteLine(message);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogCritical(string message)
        {
            Console.WriteLine(message);
        }

        public void LogCritical(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogCritical(Exception exception, string message)
        {
            Console.WriteLine(message);
        }

        public void LogCritical(Exception exception, string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void Log(LogLevel level, string message)
        {
            Console.WriteLine(message);
        }

        public void Log(LogLevel level, string message, object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void Log(LogLevel level, Exception exception, string message)
        {
            Console.WriteLine(message);
        }

        public void Log(LogLevel level, Exception exception, string message, object[] args)
        {
            Console.WriteLine(message, args);
        }

        public bool IsEnabled(LogLevel level)
        {
            return true;
        }
    }
}
