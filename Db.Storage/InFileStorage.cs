using System.IO;
using System.Threading.Tasks;
using Db.Logging;
using Db.Logging.Abstractions;
using Db.Storage.Serialization;
using Db.Utils;

namespace Db.Storage
{
    public class InFileStorage<TKey, TValue> : IStorage<TKey, TValue>
    {
        private readonly ISerialization<TValue> serialization = new JsonSerialization<TValue>();

        private readonly string path;

        private readonly ILog log;

        public InFileStorage(string name = null, ILog log = null)
        {
            path = $"storage/{name ?? typeof(TValue).Name}/";
            Directory.CreateDirectory(path);
            this.log = log == null ? new FakeLog() : log.WithPrefix($"InFileStorage({typeof(TValue).Name})");
        }

        public async Task<Result> CreateOrUpdateAsync(TKey key, TValue value)
        {
            var serializedValue = serialization.Serialize(value);
            using (var file = File.CreateText(GetFileName(key)))
            {
                await file.WriteAsync(serializedValue).ConfigureAwait(false);
            }
            
            return Result.Ok();
        }

        public async Task<Result<TValue>> GetAsync(TKey key)
        {
            var fileName = GetFileName(key);
            if (!File.Exists(fileName))
                return "No data";
            
            using (var file = File.OpenText(GetFileName(key)))
            {
                var str = await file.ReadToEndAsync().ConfigureAwait(false);
                return serialization.Deserialize(str);
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