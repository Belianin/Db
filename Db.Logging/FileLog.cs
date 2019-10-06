using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Db.Logging.Abstractions;

namespace Db.Logging
{
    [Obsolete("Two streams with buffer")]
    public class FileLog : BaseLog
    {
        private readonly ICollection<LogEvent> events;
        
        public FileLog(string fileName = null)
        {
            if (fileName == null)
                fileName = $"log-{DateTime.Now.ToString("yyyy-MMM", new CultureInfo("en-US"))}.txt";

            events = new List<LogEvent>();
            var cts = new CancellationTokenSource();
            Task.Run(() => WriteToFileTask(fileName, cts.Token));
        }
        
        protected override void Log(LogEvent logEvent)
        {
            lock (events)
            {
                events.Add(logEvent);
            }
        }

        private void WriteToFileTask(string filename, CancellationToken token)
        {
            var sb = new StringBuilder();
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(1000 * 60);
                lock (events)
                {
                    if (events.Count == 0)
                        continue;

                    foreach (var logEvent in events)
                        sb.AppendLine(LogFormatter.Format(logEvent));
                    events.Clear();
                }

                using (var textWriter = File.AppendText(filename))
                {
                    textWriter.Write(sb.ToString());
                }
                sb.Clear();
            }
        }
    }
}