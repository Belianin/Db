using System;

namespace Db.Metrics.Counter
{
    public class CounterEvent<T>
    {
        public T Value { get; }
        
        public DateTime DateTime { get; }

        public CounterEvent(T value)
        {
            Value = value;
            DateTime = DateTime.UtcNow;
        }
    }
}