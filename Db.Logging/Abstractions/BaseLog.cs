using System;

namespace Db.Logging.Abstractions
{
    public abstract class BaseLog : ILog
    {
        public void Info(string text)
        {
            if (IsInfoEnabled)
                Log(new LogEvent(LogEventLevel.INFO, DateTime.Now, text));
        }

        public void Debug(string text)
        {
            if (IsDebugEnabled)
                Log(new LogEvent(LogEventLevel.DEBUG, DateTime.Now, text));
        }

        public void Error(string text)
        {
            if (IsErrorEnabled)
                Log(new LogEvent(LogEventLevel.ERROR, DateTime.Now, text));
        }

        public void Error(Exception exception)
        {
            if (IsErrorEnabled)
                Log(new LogEvent(LogEventLevel.ERROR, DateTime.Now, exception.Message));
        }
        
        public void Error(string text, Exception exception)
        {
            if (IsErrorEnabled)
                Log(new LogEvent(LogEventLevel.ERROR, DateTime.Now, $"{text} {exception.Message}"));
        }

        public void Warn(string text)
        {
            if (IsWarnEnabled)
                Log(new LogEvent(LogEventLevel.WARN, DateTime.Now, text));
        }

        public void Fatal(string text)
        {
            if (IsFatalEnabled)
                Log(new LogEvent(LogEventLevel.FATAL, DateTime.Now, text));
        }

        public void Custom(string text)
        {
            Log(new LogEvent(LogEventLevel.FATAL, DateTime.Now, text));
        }

        protected abstract void Log(LogEvent logEvent);
        
        public bool IsInfoEnabled { get; set; } = true;
        
        public bool IsWarnEnabled { get; set; } = true;
        
        public bool IsFatalEnabled { get; set; } = true;
        
        public bool IsDebugEnabled { get; set; } = true;
        
        public bool IsErrorEnabled { get; set; } = true;
    }
}