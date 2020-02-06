﻿using System;
using Digizuite.Models;

namespace Digizuite
{
    public interface ILogger
    {
        void LogTrace(string message);
        void LogTrace(string message, params object[] args);
        void LogTrace(Exception exception, string message);
        void LogTrace(Exception exception, string message, params object[] args);
        void LogDebug(string message);
        void LogDebug(string message, params object[] args);
        void LogDebug(Exception exception, string message);
        void LogDebug(Exception exception, string message, params object[] args);
        void LogInformation(string message);
        void LogInformation(string message, params object[] args);
        void LogInformation(Exception exception, string message);
        void LogInformation(Exception exception, string message, params object[] args);
        void LogWarning(string message);
        void LogWarning(string message, params object[] args);
        void LogWarning(Exception exception, string message);
        void LogWarning(Exception exception, string message, params object[] args);
        void LogError(string message);
        void LogError(string message, params object[] args);
        void LogError(Exception exception, string message);
        void LogError(Exception exception, string message, params object[] args);
        void LogCritical(string message);
        void LogCritical(string message, params object[] args);
        void LogCritical(Exception exception, string message);
        void LogCritical(Exception exception, string message, params object[] args);

        void Log(LogLevel level, string message);
        void Log(LogLevel level, string message, object[] args);
        void Log(LogLevel level, Exception exception, string message);
        void Log(LogLevel level, Exception exception, string message, object[] args);

        bool IsEnabled(LogLevel level);
    }

    // ReSharper disable once UnusedTypeParameter
    public interface ILogger<out T> : ILogger
    {
    }
}