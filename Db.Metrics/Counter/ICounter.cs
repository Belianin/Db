namespace Db.Metrics.Counter
{
    public interface ICounter<in T>
    {
        void Add(T value);

        void Subtract(T value);

        void Equate(T value);
    }
}