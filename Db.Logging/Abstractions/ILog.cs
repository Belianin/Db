using System;

namespace Db.Logging.Abstractions
{
    public interface ILog
    {
        void Info(string text);

        void Warn(string text);

        void Fatal(string text);

        void Debug(string text);

        void Error(string text);

        void Error(Exception exception);

        void Error(string text, Exception exception);

        void Custom(string text);
        
        bool IsInfoEnabled { get; set; }
        
        bool IsWarnEnabled { get; set; }
        
        bool IsFatalEnabled { get; set; }
        
        bool IsDebugEnabled { get; set; }
        
        bool IsErrorEnabled { get; set; }
    }
}