using System.Collections.Concurrent;
using System.Threading.Tasks;
using Db.Logging.Abstractions;
using Db.Utils;

namespace Db.Storage
{
    public class InMemoryStorage<TKey, TValue> : IStorage<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> storage;

        private readonly ILog log;

        public InMemoryStorage(ILog log)
        {
            this.storage = new ConcurrentDictionary<TKey, TValue>();
            this.log = log;
        }

        public Task<Result> CreateOrUpdateAsync(TKey key, TValue value)
        {
            storage[key] = value;
            log.Debug($"{key}:{typeof(TValue)} created");
            return Task.FromResult(Result.Ok());
        }

        public async Task<Result<TValue>> GetAsync(TKey key)
        {
            log.Debug($"Trying to get {typeof(TValue)} by key {key}");
            if (storage.TryGetValue(key, out var value))
            {
                log.Debug($"Got object {key}");
                return value;
            }
            
            log.Debug($"No {typeof(TValue)} with key {key}");
            return "No key";
        }

        public Task<Result> DeleteAsync(TKey key)
        {
            storage.TryRemove(key, out var value);
            return Task.FromResult(Result.Ok());
        }
    }
}