using System;

namespace Db.Logging.Abstractions
{
    public class PrefixLog : ILog
    {
        private readonly ILog innerLog;

        private readonly string prefix;
        
        public PrefixLog(ILog log, string prefix)
        {
            innerLog = log;
            this.prefix = prefix;
        }
        
        public void Info(string text)
        {
            innerLog.Info(AddPrefix(text));
        }

        public void Warn(string text)
        {
            innerLog.Warn(AddPrefix(text));
        }

        public void Fatal(string text)
        {
            innerLog.Fatal(AddPrefix(text));
        }

        public void Debug(string text)
        {
            innerLog.Debug(AddPrefix(text));
        }

        public void Error(string text)
        {
            innerLog.Error(AddPrefix(text));
        }

        public void Error(Exception exception)
        {
            innerLog.Error(AddPrefix(exception.Message));
        }

        public void Error(string text, Exception exception)
        {
            innerLog.Error(AddPrefix($"{text} {exception.Message}"));
        }

        public void Custom(string text)
        {
            innerLog.Custom(AddPrefix(text));
        }

        public bool IsInfoEnabled { get; set; }
        public bool IsWarnEnabled { get; set; }
        public bool IsFatalEnabled { get; set; }
        public bool IsDebugEnabled { get; set; }
        public bool IsErrorEnabled { get; set; }

        private string AddPrefix(string text)
        {
            return $"[{prefix}] {text}";
        }
    }
}