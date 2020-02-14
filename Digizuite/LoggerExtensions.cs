using System;
using Digizuite.Models;
// ReSharper disable MemberCanBePrivate.Global

namespace Digizuite
{
    public static class LoggerExtensions
    {
        public static void LogTrace(this ILogger logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Trace, message);
        }

        public static void LogTrace(this ILogger logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Trace, message, args);
        }

        public static void LogTrace(this ILogger logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Trace, exception, message);
        }

        public static void LogTrace(this ILogger logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Trace, exception, message, args);
        }

        public static void LogDebug(this ILogger logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, message);
        }

        public static void LogDebug(this ILogger logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, message, args);
        }

        public static void LogDebug(this ILogger logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, exception, message);
        }

        public static void LogDebug(this ILogger logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, exception, message, args);
        }

        public static void LogInformation(this ILogger logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Information, message);
        }

        public static void LogInformation(this ILogger logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Information, message, args);
        }

        public static void LogInformation(this ILogger logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Information, exception, message);
        }

        public static void LogInformation(this ILogger logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Information, exception, message, args);
        }

        public static void LogWarning(this ILogger logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Warning, message);
        }

        public static void LogWarning(this ILogger logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Warning, message, args);
        }

        public static void LogWarning(this ILogger logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Warning, exception, message);
        }

        public static void LogWarning(this ILogger logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Warning, exception, message, args);
        }

        public static void LogError(this ILogger logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Error, message);
        }

        public static void LogError(this ILogger logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Error, message, args);
        }

        public static void LogError(this ILogger logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Error, exception, message);
        }

        public static void LogError(this ILogger logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Error, exception, message, args);
        }

        public static void LogCritical(this ILogger logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Critical, message);
        }

        public static void LogCritical(this ILogger logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Critical, message, args);
        }

        public static void LogCritical(this ILogger logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Critical, exception, message);
        }

        public static void LogCritical(this ILogger logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Critical, exception, message, args);
        }

        public static void Log(this ILogger logger, LogLevel level, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(level, null, message, Array.Empty<object>());
        }

        public static void Log(this ILogger logger, LogLevel level, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(level, null, message, args);
        }

        public static void Log(this ILogger logger, LogLevel level, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(level, exception, message, Array.Empty<object>());
        }
    }
}