using Db.Metrics.Counter.Double;
using Db.Metrics.Counter.Integer;

namespace Db.Metrics.Counter
{
    public static class CounterExtensions
    {
        public static void Increment(this IIntegerCounter counter)
        {
            counter.Add(1);
        }
        
        public static void Increment(this IDoubleCounter counter)
        {
            counter.Add(1);
        }
        
        public static void Decrement(this IIntegerCounter counter)
        {
            counter.Subtract(1);
        }
        
        public static void Decrement(this IDoubleCounter counter)
        {
            counter.Subtract(1);
        }
    }
}