using System;
using System.Collections.Generic;
using Digizuite.Models;
using Newtonsoft.Json;

namespace Digizuite.Samples
{
    public class ConsoleLogger<T> : ILogger<T>
    {
        public void Log(LogLevel level, Exception? exception, string message, params object[] args)
        {
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