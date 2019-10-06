using System.Collections.Concurrent;
using System.Threading.Tasks;
using Db.Logging;
using Db.Logging.Abstractions;
using Db.Utils;

namespace Db.Storage
{
    public class InMemoryStorage<TKey, TValue> : IStorage<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> storage;

        private readonly ILog log;

        public InMemoryStorage(ILog log = null)
        {
            this.storage = new ConcurrentDictionary<TKey, TValue>();
            this.log = log == null ? new FakeLog() : log.WithPrefix($"InMemoryStorage({typeof(TValue).Name})");
        }

        public Task<Result> CreateOrUpdateAsync(TKey key, TValue value)
        {
            log.Debug($"Trying to create or update value for the key {key.ToString()}");
            var existsBefore = storage.ContainsKey(key);
            storage[key] = value;
            log.Debug(existsBefore
                ? $"\"{key}:{typeof(TValue).Name}\" updated"
                : $"\"{key}:{typeof(TValue).Name}\" created");
            
            return Task.FromResult(Result.Ok());
        }

        public Task<Result<TValue>> GetAsync(TKey key)
        {
            log.Debug($"Trying to get value for the key {key.ToString()}");
            if (storage.TryGetValue(key, out var value))
            {
                log.Debug($"Got value for the key {key.ToString()}");
                return Task.FromResult(Result<TValue>.Ok(value));
            }

            var noKeyMessage = FormNoKeyMessage(key);
            log.Debug(noKeyMessage);
            
            return Task.FromResult(Result<TValue>.Fail(noKeyMessage));
        }

        public Task<Result> DeleteAsync(TKey key)
        {
            log.Debug($"Trying to delete value for the key {key.ToString()}");
            return Task.FromResult(storage.TryRemove(key, out var value) 
                ? Result.Fail(FormNoKeyMessage(key)) 
                : Result.Ok());
        }

        private static string FormNoKeyMessage(TKey key) => $"No value for the key \"{key.ToString()}\"";
    }
}