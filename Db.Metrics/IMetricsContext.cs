using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Db.Metrics.Counter;
using Db.Metrics.Counter.Integer;

namespace Db.Metrics
{
    public interface IMetricsContext
    {
        IIntegerCounter CreateIntegerCounter(string name, TimeSpan aggregationPeriod);

        void ReceiveEvents(string name, ICollection<CounterEvent<int>> events);
    }

    public class MetricsContext : IMetricsContext
    {
        private readonly ConcurrentDictionary<string, List<CounterEvent<int>>> integerCounterEvents;

        private readonly ICollection<(ICounter, TimeSpan)> counters;

        public MetricsContext()
        {
            integerCounterEvents = new ConcurrentDictionary<string, List<CounterEvent<int>>>();
            counters = new List<(ICounter, TimeSpan)>();
        }

        public IIntegerCounter CreateIntegerCounter(string name, TimeSpan aggregationPeriod)
        {
            if (integerCounterEvents.ContainsKey(name))
                throw new ArgumentException($"\"{name}\" counter is already exists");
            
            integerCounterEvents[name] = new List<CounterEvent<int>>();
            var counter = new IntegerCounter(name);
            counters.Add((counter, aggregationPeriod));

            return counter;
        }

        public void ReceiveEvents(string name, ICollection<CounterEvent<int>> events)
        {
            integerCounterEvents[name].AddRange(events);
        }
    }
}