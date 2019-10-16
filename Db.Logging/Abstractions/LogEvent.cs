using System;
using System.ComponentModel;

namespace Db.Logging.Abstractions
{
    public class LogEvent
    {
        public LogLevel Level { get; }
        
        public DateTime DateTime { get; }
        
        public string Message { get; }
        
        public string Author { get; }

        public LogEvent(LogLevel level, string message, string author = null)
        {
            Level = level;
            Message = message;
            Author = author;
            DateTime = DateTime.Now;
        }
    }

    public enum LogLevel
    {
        [Description("INFO")]
        Info,
        [Description("WARN")]
        Warning,
        [Description("ERROR")]
        Error,
        [Description("FATAL")]
        Fatal,
        [Description("DEBUG")]
        Debug,
        [Description("CUSTOM")]
        Custom
    }
}