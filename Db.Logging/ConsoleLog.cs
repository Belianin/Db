using System;
using Db.Logging.Abstractions;

namespace Db.Logging
{
    public class ConsoleLog : BaseLog
    {
        protected override void Log(LogEvent logEvent)
        {
            var text = LogFormatter.Format(logEvent);
            Console.WriteLine(text);
        }
    }
}