using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Db.Logging.Abstractions;

namespace Db.Logging
{
    public class FileLog : BaseLog, IDisposable
    {
        private readonly ICollection<LogEvent> events;

        private readonly CancellationTokenSource cts;

        private readonly string fileName;
        
        private readonly TimeSpan aggregationPeriod = TimeSpan.FromMinutes(1);
        
        public FileLog(string fileName = null)
        {
            if (fileName == null)
                this.fileName = $"log-{DateTime.Now.ToString("yyyy-MMM", new CultureInfo("en-US"))}.txt";

            events = new List<LogEvent>();
            cts = new CancellationTokenSource();
            
            Task.Run(() => WriteToFileTask(cts.Token));
        }

        public void Dispose()
        {
            cts.Cancel();
            cts.Dispose();

            if (events.Count == 0)
                return;
            
            using (var textWriter = File.AppendText(fileName))
            {
                textWriter.Write(events.Select(s => $"{LogFormatter.Format(s)}\n"));
            }
        }

        protected override void Log(LogEvent logEvent)
        {
            lock (events)
            {
                events.Add(logEvent);
            }
        }

        private void WriteToFileTask(CancellationToken token)
        {
            var sb = new StringBuilder();
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(aggregationPeriod);
                lock (events)
                {
                    if (events.Count == 0)
                        continue;

                    foreach (var logEvent in events)
                        sb.AppendLine(LogFormatter.Format(logEvent));
                    events.Clear();
                }

                using (var textWriter = File.AppendText(fileName))
                {
                    textWriter.Write(sb.ToString());
                }
                sb.Clear();
            }
        }
    }
}