using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Db.Storage.Keyless;

namespace Db.Metrics.Counter.Integer
{
    public class IntegerCounter : IIntegerCounter
    {
        private int currentValue;

        private readonly List<CounterEvent<int>> events;

        private readonly TimeSpan aggregationPeriod;

        internal IntegerCounter(
            TimeSpan aggregationPeriod,
            List<CounterEvent<int>> events)
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
                events.Add(new CounterEvent<int>(currentValue));
                currentValue = 0;
            }
        }
    }
}