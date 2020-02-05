using System;
using System.Collections.Generic;
using System.Text;
using Digizuite.Models;

namespace Digizuite.Samples
{
    internal class Logger : ILogger
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
            throw new NotImplementedException();
        }

        public void LogInformation(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogInformation(Exception exception, string message)
        {
            throw new NotImplementedException();
        }

        public void LogInformation(Exception exception, string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogWarning(string message)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void LogWarning(Exception exception, string message)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogError(string message)
        {
            throw new NotImplementedException();
        }

        public void LogError(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogError(Exception exception, string message)
        {
            throw new NotImplementedException();
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogCritical(string message)
        {
            throw new NotImplementedException();
        }

        public void LogCritical(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogCritical(Exception exception, string message)
        {
            throw new NotImplementedException();
        }

        public void LogCritical(Exception exception, string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level, string message)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level, string message, object[] args)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level, Exception exception, string message)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level, Exception exception, string message, object[] args)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel level)
        {
            throw new NotImplementedException();
        }
    }
}
