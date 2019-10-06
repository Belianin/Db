using System.Collections.Generic;

namespace Db.Metrics.Counter.Integer
{
    public class IntegerCounter : IIntegerCounter
    {
        private int currentValue;

        private readonly string name;

        private readonly List<CounterEvent<int>> events;

        internal IntegerCounter(string name)
        {
            this.name = name;
            events = new List<CounterEvent<int>>();
        }

        public void Accept(IMetricsContext context)
        {
            context.ReceiveEvents(name, events.ToArray());
            events.Clear();
        }
        
        public void Add(int value)
        {
            currentValue += value;
            events.Add(new CounterEvent<int>(currentValue));
        }

        public void Subtract(int value)
        {
            currentValue -= value;
            events.Add(new CounterEvent<int>(currentValue));
        }

        public void Equate(int value)
        {
            currentValue = value;
            events.Add(new CounterEvent<int>(currentValue));
        }
    }
}