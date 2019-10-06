using System.IO;
using System.Threading.Tasks;
using Db.Storage.Serialization;
using Db.Utils;

namespace Db.Storage
{
    public class CustomStorage<TKey> : IStorage<TKey>
    {
        private readonly ISerialization serialization = new JsonSerialization();

        private readonly string path;

        public CustomStorage(string name)
        {
            path = $"storage/{name}/";
            Directory.CreateDirectory(path);
        }
        
        public async Task<Result> CreateOrUpdateAsync<T>(TKey key, T value)
        {
            var serializedValue = serialization.Serialize(value);
            using (var file = File.CreateText(GetFileName(key)))
            {
                await file.WriteAsync(serializedValue).ConfigureAwait(false);
            }
            
            return Result.Ok();
        }

        public async Task<Result<T>> GetAsync<T>(TKey key)
        {
            var fileName = GetFileName(key);
            if (!File.Exists(fileName))
                return "No data";
            
            using (var file = File.OpenText(GetFileName(key)))
            {
                var str = await file.ReadToEndAsync().ConfigureAwait(false);
                return serialization.Deserialize<T>(str);
            }
        }
        
        public Task<Result> DeleteAsync(TKey key)
        {
            var fileName = GetFileName(key);
            if (!File.Exists(fileName))
                return Task.FromResult(Result.Ok());
            
            File.Delete(fileName);
            return Task.FromResult(Result.Ok());
        }

        private string GetFileName(TKey key)
        {
            return $"{path}{key.ToString()}";
        }
    }
}