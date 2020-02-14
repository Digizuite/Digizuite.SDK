using System;
using Digizuite.Models;
// ReSharper disable MemberCanBePrivate.Global

namespace Digizuite
{
    public static class LoggerExtensions
    {
        public static void LogTrace<T>(this ILogger<T> logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Trace, message);
        }

        public static void LogTrace<T>(this ILogger<T> logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Trace, message, args);
        }

        public static void LogTrace<T>(this ILogger<T> logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Trace, exception, message);
        }

        public static void LogTrace<T>(this ILogger<T> logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Trace, exception, message, args);
        }

        public static void LogDebug<T>(this ILogger<T> logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, message);
        }

        public static void LogDebug<T>(this ILogger<T> logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, message, args);
        }

        public static void LogDebug<T>(this ILogger<T> logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, exception, message);
        }

        public static void LogDebug<T>(this ILogger<T> logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, message, args);
        }

        public static void LogInformation<T>(this ILogger<T> logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Information, message);
        }

        public static void LogInformation<T>(this ILogger<T> logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Information, message, args);
        }

        public static void LogInformation<T>(this ILogger<T> logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Information, exception, message);
        }

        public static void LogInformation<T>(this ILogger<T> logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Information, exception, message, args);
        }

        public static void LogWarning<T>(this ILogger<T> logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Warning, message);
        }

        public static void LogWarning<T>(this ILogger<T> logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Warning, message, args);
        }

        public static void LogWarning<T>(this ILogger<T> logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Warning, exception, message);
        }

        public static void LogWarning<T>(this ILogger<T> logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Warning, message, args);
        }

        public static void LogError<T>(this ILogger<T> logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Error, message);
        }

        public static void LogError<T>(this ILogger<T> logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Error, message, args);
        }

        public static void LogError<T>(this ILogger<T> logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Error, exception, message);
        }

        public static void LogError<T>(this ILogger<T> logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Error, message, args);
        }

        public static void LogCritical<T>(this ILogger<T> logger, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Critical, message);
        }

        public static void LogCritical<T>(this ILogger<T> logger, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Critical, message, args);
        }

        public static void LogCritical<T>(this ILogger<T> logger, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Critical, message);
        }

        public static void LogCritical<T>(this ILogger<T> logger, Exception exception, string message,
            params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Critical, message, args);
        }

        public static void Log<T>(this ILogger<T> logger, LogLevel level, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(level, null, message, Array.Empty<object>());
        }

        public static void Log<T>(this ILogger<T> logger, LogLevel level, string message, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(level, null, message, args);
        }

        public static void Log<T>(this ILogger<T> logger, LogLevel level, Exception exception, string message)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            logger.Log(level, exception, message, Array.Empty<object>());
        }
    }
}