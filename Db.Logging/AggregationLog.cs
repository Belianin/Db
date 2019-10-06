using System;
using System.Collections.Generic;
using System.Linq;
using Db.Logging.Abstractions;

namespace Db.Logging
{
    public class AggregationLog : ILog
    {
        private readonly IList<ILog> logs;

        public AggregationLog(IList<ILog> logs)
        {
            this.logs = logs.ToList();
        }

        public void AddLog(ILog log)
        {
            logs.Add(log);
        }

        public void Info(string text)
        {
            if (!IsInfoEnabled)
                return;
            foreach (var log in logs) 
                log.Info(text);
        }
        
        public void Debug(string text)
        {
            if (!IsDebugEnabled)
                return;
            foreach (var log in logs) 
                log.Debug(text);
        }

        public void Error(string text)
        {
            if (!IsErrorEnabled)
                return;
            foreach (var log in logs) 
                log.Error(text);
        }

        public void Error(Exception exception)
        {
            if (!IsErrorEnabled)
                return;
            foreach (var log in logs) 
                log.Error(exception);
        }

        public void Error(string text, Exception exception)
        {
            if (!IsErrorEnabled)
                return;
            foreach (var log in logs) 
                log.Error(text, exception);
        }

        public void Warn(string text)
        {
            if (!IsWarnEnabled)
                return;
            foreach (var log in logs) 
                log.Warn(text);
        }

        public void Fatal(string text)
        {
            if (!IsFatalEnabled)
                return;
            foreach (var log in logs) 
                log.Fatal(text);
        }
        
        public void Custom(string text)
        {
            foreach (var log in logs) 
                log.Custom(text);
        }
        
        public bool IsInfoEnabled { get; set; } = true;
        
        public bool IsWarnEnabled { get; set; } = true;
        
        public bool IsFatalEnabled { get; set; } = true;
        
        public bool IsDebugEnabled { get; set; } = true;
        
        public bool IsErrorEnabled { get; set; } = true;
    }
}