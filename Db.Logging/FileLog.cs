using System;
using System.Globalization;
using System.IO;
using Db.Logging.Abstractions;

namespace Db.Logging
{
    public class FileLog : BaseLog, IDisposable
    {
        private readonly StreamWriter writer;
        
        public FileLog(string fileName = null)
        {
            if (fileName == null)
                fileName = $"log-{DateTime.Now.ToString("yyyy-MMM", new CultureInfo("en-US"))}.txt";

            if (!File.Exists(fileName))
                File.Create(fileName);
            
            writer = new StreamWriter(new BufferedStream(File.OpenWrite(fileName)));
        }

        public void Dispose()
        {
            writer.Dispose();
        }

        protected override void Log(LogEvent logEvent)
        {
            writer.WriteLine(LogFormatter.Format(logEvent));
        }
    }
}