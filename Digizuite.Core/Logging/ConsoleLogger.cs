﻿using System;
using System.Collections.Generic;
using Digizuite.Models;
using Newtonsoft.Json;

namespace Digizuite.Logging
{
    /// <summary>
    /// Provides a convenient and simple implementation of the ILogger interface, for local development.
    /// Extend the class with your own custom subclass if you want to be able to control the loglevel.
    /// </summary>
    public class ConsoleLogger<T> : ILogger<T>
    {
        public bool IsLogLevelEnabled(LogLevel level)
        {
            return true;
        }

        public void Log(LogLevel level, Exception? exception, string message, params object?[] args)
        {
            if (!IsLogLevelEnabled(level))
            {
                return;
            }

            var logLine = new Dictionary<string, object>
            {
                {"level", level.ToString()},
                {"message", message},
                {"args", args},
            };

            if (exception != null)
            {
                logLine["exception"] = exception;
            }
            
            Console.WriteLine(JsonConvert.SerializeObject(logLine));
        }
    }
}