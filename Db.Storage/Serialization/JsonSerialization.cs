using Newtonsoft.Json;

namespace Db.Storage.Serialization
{
    public class JsonSerialization<TValue> : ISerialization<TValue>
    {
        public string Serialize(TValue value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public TValue Deserialize(string value)
        {
            return JsonConvert.DeserializeObject<TValue>(value);
        }
    }
    
    public class JsonSerialization : ISerialization
    {
        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}