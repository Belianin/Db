using System;
using Db.Logging.Abstractions;

namespace Db.Logging
{
    public class FakeLog : ILog
    {
        public void Info(string text){}

        public void Error(string text){}

        public void Error(Exception exception){}

        public void Error(string text, Exception exception){}

        public void Warn(string text){}
        
        public void Debug(string text){}

        public void Fatal(string text){}
        
        public void Custom(string text){}
        
        public bool IsInfoEnabled { get; set; }
        
        public bool IsWarnEnabled { get; set; }
        
        public bool IsFatalEnabled { get; set; }
        
        public bool IsDebugEnabled { get; set; }
        
        public bool IsErrorEnabled { get; set; }
    }
}