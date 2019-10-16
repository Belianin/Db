using System;

namespace Db.Logging.Abstractions
{
    public abstract class BaseLog : ILog
    {
        public void Info(string text)
        {
            if (IsInfoEnabled)
                Log(new LogEvent(LogLevel.Info, text));
        }

        public void Debug(string text)
        {
            if (IsDebugEnabled)
                Log(new LogEvent(LogLevel.Debug, text));
        }

        public void Error(string text)
        {
            if (IsErrorEnabled)
                Log(new LogEvent(LogLevel.Error, text));
        }

        public void Error(Exception exception)
        {
            if (IsErrorEnabled)
                Log(new LogEvent(LogLevel.Error, exception.Message));
        }
        
        public void Error(string text, Exception exception)
        {
            if (IsErrorEnabled)
                Log(new LogEvent(LogLevel.Error, $"{text} {exception.Message}"));
        }

        public void Warn(string text)
        {
            if (IsWarnEnabled)
                Log(new LogEvent(LogLevel.Warning, text));
        }

        public void Fatal(string text)
        {
            if (IsFatalEnabled)
                Log(new LogEvent(LogLevel.Fatal, text));
        }

        public void Custom(string text)
        {
            Log(new LogEvent(LogLevel.Fatal, text));
        }

        protected abstract void Log(LogEvent logEvent);
        
        public bool IsInfoEnabled { get; set; } = true;
        
        public bool IsWarnEnabled { get; set; } = true;
        
        public bool IsFatalEnabled { get; set; } = true;
        
        public bool IsDebugEnabled { get; set; } = true;
        
        public bool IsErrorEnabled { get; set; } = true;
    }
}