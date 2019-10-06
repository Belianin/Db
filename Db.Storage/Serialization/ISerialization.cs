namespace Db.Storage.Serialization
{
    public interface ISerialization<TValue>
    {
        string Serialize(TValue value);

        TValue Deserialize(string value);
    }
    
    public interface ISerialization
    {
        string Serialize<T>(T value);

        T Deserialize<T>(string value);
    }
}