using System;
using System.Threading;
using System.Threading.Tasks;
using Db.Storage.Keyless;

namespace Db.Metrics.Counter.Integer
{
    public class IntegerCounter : IIntegerCounter
    {
        private int currentValue;

        private readonly IKeylessStorage<CounterEvent<int>> events;

        private readonly TimeSpan aggregationPeriod;

        internal IntegerCounter(
            TimeSpan aggregationPeriod,
            IKeylessStorage<CounterEvent<int>> events)
        {
            this.aggregationPeriod = aggregationPeriod;
            this.events = events;
            Task.Run(SendValue);
        }
        
        public void Add(int value)    
        {
            currentValue += value;
        }

        public void Subtract(int value)
        {
            currentValue -= value;
        }

        public void Equate(int value)
        {
            currentValue = value;
        }

        private void SendValue()
        {
            // todo Interlocked
            while (true)
            {
                Thread.Sleep(aggregationPeriod);
                events.Create(new CounterEvent<int>(currentValue));
                currentValue = 0;
            }
        }
    }
}