namespace Db.Metrics.Counter
{
    public interface ICounter<in T> : ICounter
    {
        void Add(T value);

        void Subtract(T value);

        void Equate(T value);
    }
    
    public interface ICounter
    {
        void Accept(IMetricsContext context);
    }
}