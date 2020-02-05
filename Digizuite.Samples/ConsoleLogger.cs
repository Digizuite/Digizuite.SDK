using System;
using System.Collections.Generic;
using Digizuite.Models;
using Newtonsoft.Json;

namespace Digizuite.Samples
{
    public class ConsoleLogger<T> : ILogger<T>
    {
        public void LogTrace(string message)
        {
            Log(LogLevel.Trace, message);
        }

        public void LogTrace(string message, params object[] args)
        {
            Log(LogLevel.Trace, message, args);
        }

        public void LogTrace(Exception exception, string message)
        {
            Log(LogLevel.Trace, exception, message);
        }

        public void LogTrace(Exception exception, string message, params object[] args)
        {
            Log(LogLevel.Trace, exception, message, args);
        }

        public void LogDebug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void LogDebug(string message, params object[] args)
        {
            Log(LogLevel.Debug, message, args);
        }

        public void LogDebug(Exception exception, string message)
        {
            Log(LogLevel.Debug, exception, message);
        }

        public void LogDebug(Exception exception, string message, params object[] args)
        {
            Log(LogLevel.Debug, message, args);
        }

        public void LogInformation(string message)
        {
            Log(LogLevel.Information, message);
        }

        public void LogInformation(string message, params object[] args)
        {
            Log(LogLevel.Information, message, args);
        }

        public void LogInformation(Exception exception, string message)
        {
            Log(LogLevel.Information, exception, message);
        }

        public void LogInformation(Exception exception, string message, params object[] args)
        {
            Log(LogLevel.Information, exception, message, args);
        }

        public void LogWarning(string message)
        {
            Log(LogLevel.Warning, message);
        }

        public void LogWarning(string message, params object[] args)
        {
            Log(LogLevel.Warning, message, args);
        }

        public void LogWarning(Exception exception, string message)
        {
            Log(LogLevel.Warning, exception, message);
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            Log(LogLevel.Warning, message, args);
        }

        public void LogError(string message)
        {
            Log(LogLevel.Error, message);
        }

        public void LogError(string message, params object[] args)
        {
            Log(LogLevel.Error, message, args);
        }

        public void LogError(Exception exception, string message)
        {
            Log(LogLevel.Error, exception, message);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            Log(LogLevel.Error, message, args);
        }

        public void LogCritical(string message)
        {
            Log(LogLevel.Critical, message);
        }

        public void LogCritical(string message, params object[] args)
        {
            Log(LogLevel.Critical, message, args);
        }

        public void LogCritical(Exception exception, string message)
        {
            Log(LogLevel.Critical, message);
        }

        public void LogCritical(Exception exception, string message, params object[] args)
        {
            Log(LogLevel.Critical, message, args);
        }

        public void Log(LogLevel level, string message)
        {
            InnerLog(level, message, null, new object[0]);
        }

        public void Log(LogLevel level, string message, object[] args)
        {
            InnerLog(level, message, null, args);
        }

        public void Log(LogLevel level, Exception exception, string message)
        {
            InnerLog(level, message, exception, new object[0]);
        }

        public void Log(LogLevel level, Exception exception, string message, object[] args)
        {
            InnerLog(level, message, exception, args);
        }

        public bool IsEnabled(LogLevel level)
        {
            return true;
        }

        private void InnerLog(LogLevel level, string message, Exception exception, object[] args)
        {
            Console.WriteLine(JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                {"level", level.ToString()},
                {"message", message},
                {"args", args},
                {"exception", exception}
            }));
        }
    }
}
