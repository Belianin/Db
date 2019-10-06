using System;
using Db.Logging.Abstractions;

namespace Db.Logging
{
    public class ColorConsoleLog : ILog
    {
        public void Info(string text)
        {
            if (!IsInfoEnabled)
                return;
            var logEvent = new LogEvent(LogEventLevel.INFO, DateTime.Now, text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(LogFormatter.Format(logEvent));
        }

        public void Warn(string text)
        {
            if (!IsWarnEnabled)
                return;
            var logEvent = new LogEvent(LogEventLevel.WARN, DateTime.Now, text);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(LogFormatter.Format(logEvent));
        }

        public void Fatal(string text)
        {
            if (!IsFatalEnabled)
                return;
            var logEvent = new LogEvent(LogEventLevel.FATAL, DateTime.Now, text);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(LogFormatter.Format(logEvent));
        }

        public void Debug(string text)
        {
            if (!IsDebugEnabled)
                return;
            var logEvent = new LogEvent(LogEventLevel.DEBUG, DateTime.Now, text);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(LogFormatter.Format(logEvent));
        }

        public void Error(string text)
        {
            if (!IsErrorEnabled)
                return;
            var logEvent = new LogEvent(LogEventLevel.ERROR, DateTime.Now, text);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(LogFormatter.Format(logEvent));
        }

        public void Error(Exception exception)
        {
            if (!IsErrorEnabled)
                return;
            var logEvent = new LogEvent(LogEventLevel.ERROR, DateTime.Now, exception.Message);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(LogFormatter.Format(logEvent));
        }

        public void Error(string text, Exception exception)
        {
            if (!IsErrorEnabled)
                return;
            var logEvent = new LogEvent(LogEventLevel.ERROR, DateTime.Now, $"{text} {exception.Message}");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(LogFormatter.Format(logEvent));
        }

        public void Custom(string text)
        {
            var logEvent = new LogEvent(LogEventLevel.CUSTOM, DateTime.Now, text);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(LogFormatter.Format(logEvent));
        }

        public bool IsInfoEnabled { get; set; }
        public bool IsWarnEnabled { get; set; }
        public bool IsFatalEnabled { get; set; }
        public bool IsDebugEnabled { get; set; }
        public bool IsErrorEnabled { get; set; }
    }
}