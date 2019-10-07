using System;
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
        private readonly InFileStorage<TKey> innerStorage;

        public InFileStorage(string name = null, ILog log = null)
        {
            var innerName = name ?? typeof(TValue).Name;
            var innerLog = log?.WithPrefix($"InFileStorage({typeof(TValue).Name})");
            
            innerStorage = new InFileStorage<TKey>(innerName, innerLog);
        }

        public async Task<Result> CreateOrUpdateAsync(TKey key, TValue value)
        {
            return await innerStorage.CreateOrUpdateAsync(key, value).ConfigureAwait(false);
        }

        public async Task<Result<TValue>> GetAsync(TKey key)
        {
            return await innerStorage.GetAsync<TValue>(key).ConfigureAwait(false);
        }

        public async Task<Result> DeleteAsync(TKey key)
        {
            return await innerStorage.DeleteAsync(key).ConfigureAwait(false);
        }
    }
    
    [Obsolete("Нужно дескриптор всегда держать открытым")]
    public class InFileStorage<TKey> : IStorage<TKey>
    {
        private readonly ISerialization serialization = new JsonSerialization {IsIndented = true};

        private readonly string path;

        private readonly ILog log;

        public InFileStorage(string name, ILog log = null)
        {
            path = $"storage/{name}/";
            Directory.CreateDirectory(path);
            this.log = log == null ? new FakeLog() : log.WithPrefix($"InFileStorage");
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
        
        public async Task<Result> DeleteAsync(TKey key)
        {
            var fileName = GetFileName(key);
            if (!File.Exists(fileName))
                return Result.Ok();
            
            File.Delete(fileName);
            return Result.Ok();
        }

        private string GetFileName(TKey key)
        {
            return $"{path}{key.ToString()}";
        }
    }
}