using Newtonsoft.Json;

namespace Db.Storage.Serialization
{
    public class JsonSerialization : ISerialization
    {
        public bool IsIndented { get; set; }
        
        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, IsIndented ? Formatting.Indented : Formatting.None);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}