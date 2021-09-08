using System;
using System.Collections.Generic;
using Digizuite.Logging;
using Newtonsoft.Json;

namespace Digizuite.Samples.Logging
{
    public class SimpleLogger<T> : ILogger<T>
    {
        public virtual bool IsLogLevelEnabled(LogLevel level)
        {
            return true;
        }

        public void Log(LogLevel level, Exception exception, string message, params object[] args)
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