using System;
using System.Threading;
using System.Threading.Tasks;
using Db.Logging.Abstractions;

namespace Db.Metrics
{
    public class PrimitiveCounter
    {
        private readonly string name;
        
        private int currentValue;

        private readonly ILog log;

        public PrimitiveCounter(string name, ILog log)
        {
            this.name = name;
            this.log = log;
            Task.Delay(TimeSpan.FromDays(1).Subtract(DateTime.Now.TimeOfDay))
                .ContinueWith(x => PrintValue());
        }

        public void Add(int value)
        {
            currentValue += value;
        }

        private void PrintValue()
        {
            while (true)
            {
                log.Info($"[METRICS] {name}:{currentValue}");
                currentValue = 0;
                Thread.Sleep(TimeSpan.FromDays(1).Subtract(DateTime.Now.TimeOfDay));
            }
        }
    }
}