using System;
using System.Collections.Generic;
using Db.Metrics.Counter;
using Db.Metrics.Counter.Integer;
using Db.Storage.Keyless;

namespace Db.Metrics
{
    public interface IMetricsContext
    {
        IIntegerCounter CreateIntegerCounter(string name, TimeSpan aggregationPeriod);
    }

    public class MetricsContext : IMetricsContext
    {
        private readonly Dictionary<string, IKeylessStorage<CounterEvent<int>>> integerCounters;

        public MetricsContext()
        {
            integerCounters = new Dictionary<string, IKeylessStorage<CounterEvent<int>>>();
        }

        public IIntegerCounter CreateIntegerCounter(string name, TimeSpan aggregationPeriod)
        {
            if (integerCounters.ContainsKey(name))
                throw new ArgumentException($"\"{name}\" counter is already exists");
            
            var storage = new InMemoryKeylessStorage<CounterEvent<int>>();
            integerCounters[name] = storage;
            
            return new IntegerCounter(aggregationPeriod, storage);
        }
    }
}