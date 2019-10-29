using System;
using System.Collections.Generic;
using System.Linq;
using Db.Metrics.Counter;
using Db.Metrics.Counter.Integer;
using Db.Storage.Keyless;

namespace Db.Metrics
{
    public class MetricsContext : IMetricsContext
    {
        private readonly Dictionary<string, List<CounterEvent<int>>> integerCounters;

        public MetricsContext()
        {
            integerCounters = new Dictionary<string, List<CounterEvent<int>>>();
        }

        public IIntegerCounter CreateIntegerCounter(string name, TimeSpan aggregationPeriod)
        {
            if (integerCounters.ContainsKey(name))
                throw new ArgumentException($"\"{name}\" counter is already exists");
            
            var storage = new List<CounterEvent<int>>();
            integerCounters[name] = storage;
            
            return new IntegerCounter(aggregationPeriod, storage);
        }

        public IEnumerable<CounterEvent<int>> GetIntegerCountMetrics(string name)
        {
            if (!integerCounters.ContainsKey(name))
                throw new ArgumentException($"No \"{name}\" counter");
            
            return integerCounters[name];
        }
    }
}