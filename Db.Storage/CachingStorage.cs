using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Db.Logging.Abstractions;
using Db.Utils;

namespace Db.Storage
{
    public class CachingStorage<TKey, TValue> : IStorage<TKey, TValue>, IDisposable
    {
        private readonly IDictionary<TKey, int> expiration;
        
        private readonly IStorage<TKey, TValue> cache;

        private readonly IStorage<TKey, TValue> longMemory;

        private readonly ILog log;

        private readonly TimeSpan expirationPeriod = TimeSpan.FromHours(1);

        private readonly int maxExpiration = 6;

        private readonly CancellationTokenSource cts;

        public CachingStorage(string storageName, ILog log) :
            this(new InMemoryStorage<TKey, TValue>(log), 
                new InFileStorage<TKey, TValue>(storageName), 
                log) {}

        public CachingStorage(IStorage<TKey, TValue> cache, IStorage<TKey, TValue> longMemory, ILog log)
        {
            cts = new CancellationTokenSource();
            expiration = new ConcurrentDictionary<TKey, int>();
            this.cache = cache;
            this.longMemory = longMemory;
            this.log = log;
            Task.Run(() => CheckForExpiration(cts.Token));
        }

        public async Task<Result> CreateOrUpdateAsync(TKey key, TValue value)
        {
            var result = await cache.CreateOrUpdateAsync(key, value).ConfigureAwait(false);
            if (result.IsSuccess)
                expiration[key] = maxExpiration;

            return result;
        }

        public async Task<Result<TValue>> GetAsync(TKey key)
        {
            var cachedValue = await cache.GetAsync(key).ConfigureAwait(false);
            if (cachedValue.IsSuccess)
            {
                log.Debug($"key:{key.ToString()} got data from cache");
                expiration[key] = maxExpiration;
                return cachedValue.Value;
            }

            log.Debug($"key:{key.ToString()} no data in cache");
            var longValue = await longMemory.GetAsync(key).ConfigureAwait(false);
            if (longValue.IsFail)
            {
                log.Debug($"key:{key.ToString()} no data in long memory");
                return longValue.Error;
            }

            log.Debug($"key:{key.ToString()} got data from long memory");
            await cache.CreateOrUpdateAsync(key, longValue.Value).ConfigureAwait(false);

            return longValue.Value;
        }

        public async Task<Result> DeleteAsync(TKey key)
        {
            expiration.Remove(key);
            await cache.DeleteAsync(key).ConfigureAwait(false);
            await longMemory.DeleteAsync(key).ConfigureAwait(false);
            return Result.Ok();
        }

        public void Save()
        {
            Task.WaitAll(expiration.Select(pair => MoveFromCacheToLong(pair.Key)).ToArray());
        }
        
        public void Dispose()
        {
            Save();
            cts.Cancel();
        }

        private void CheckForExpiration(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {   
                Thread.Sleep(expirationPeriod);
                var tasks = new List<Task>();
                foreach (var keyValuePair in expiration)
                {
                    expiration[keyValuePair.Key]--;
                    if (keyValuePair.Value > 1) 
                        continue;
                    
                    expiration.Remove(keyValuePair.Key);
                    tasks.Add(MoveFromCacheToLong(keyValuePair.Key));
                }

                Task.WaitAll(tasks.ToArray());
            }
        }

        private async Task MoveFromCacheToLong(TKey key)
        {
            log.Debug($"key:{key.ToString()} data is expired");
            var value = await cache.GetAsync(key).ConfigureAwait(false);
            await longMemory.CreateOrUpdateAsync(key, value.Value).ConfigureAwait(false);
        }
    }
}