using System;

namespace Db.Logging.Abstractions
{
    public class LogEvent
    {
        public LogEventLevel Level { get; }
        
        public DateTime DateTime { get; }
        
        public string Message { get; }
        
        public Exception Exception { get; private set; }
        
        public Type Creator { get; }

        public LogEvent(LogEventLevel level, DateTime dateTime, string message)
        {
            Level = level;
            DateTime = dateTime;
            Message = message;
        }

        public static LogEvent FromException(Exception exception, string error = null)
        {
            return new LogEvent(LogEventLevel.ERROR, DateTime.Now, error) {Exception = exception};
        }
    }

    public enum LogEventLevel
    {
        INFO,
        WARN,
        ERROR,
        FATAL,
        DEBUG,
        CUSTOM
    }
}