using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Db.Logging;
using Db.Logging.Abstractions;
using Db.Utils;

namespace Db.Storage
{
    public class CachingStorage<TKey, TValue> : IStorage<TKey, TValue>, IDisposable
    {
        private readonly IDictionary<TKey, int> timeToLive;
        
        private readonly IStorage<TKey, TValue> cacheStorage;

        private readonly IStorage<TKey, TValue> longStorage;

        private readonly ILog log;

        private readonly TimeSpan expirationPeriod = TimeSpan.FromHours(1);

        private readonly int maxTimeToLive = 6;
        
        private readonly CancellationTokenSource cts;
        
        public bool IsAutoSaveEnabled { get; set; } = true;

        public CachingStorage() : this(typeof(TValue).Name, new FakeLog()){}

        public CachingStorage(string storageName, ILog log) :
            this(new InMemoryStorage<TKey, TValue>(log), 
                new InFileStorage<TKey, TValue>(storageName, log), 
                log) {}

        public CachingStorage(IStorage<TKey, TValue> cacheStorage, IStorage<TKey, TValue> longStorage, ILog log)
        {
            cts = new CancellationTokenSource();
            timeToLive = new ConcurrentDictionary<TKey, int>();
            this.cacheStorage = cacheStorage;
            this.longStorage = longStorage;
            this.log = log;
            Task.Run(() => CheckForExpiration(cts.Token));
        }

        public async Task<Result> CreateOrUpdateAsync(TKey key, TValue value)
        {
            var result = await cacheStorage.CreateOrUpdateAsync(key, value).ConfigureAwait(false);
            if (result.IsSuccess)
                timeToLive[key] = maxTimeToLive;

            return result;
        }

        public async Task<Result<TValue>> GetAsync(TKey key)
        {
            var cachedValue = await cacheStorage.GetAsync(key).ConfigureAwait(false);
            if (cachedValue.IsSuccess)
            {
                log.Debug($"key:{key.ToString()} got data from cache");
                timeToLive[key] = maxTimeToLive;
                return cachedValue.Value;
            }

            log.Debug($"key:{key.ToString()} no data in cache");
            var longValue = await longStorage.GetAsync(key).ConfigureAwait(false);
            if (longValue.IsFail)
            {
                log.Debug($"key:{key.ToString()} no data in long memory");
                return longValue.Error;
            }

            log.Debug($"key:{key.ToString()} got data from long memory");
            await cacheStorage.CreateOrUpdateAsync(key, longValue.Value).ConfigureAwait(false);

            return longValue.Value;
        }

        public async Task<Result> DeleteAsync(TKey key)
        {
            timeToLive.Remove(key);
            await cacheStorage.DeleteAsync(key).ConfigureAwait(false);
            await longStorage.DeleteAsync(key).ConfigureAwait(false);
            return Result.Ok();
        }

        public void Save()
        {
            Task.WaitAll(timeToLive.Select(pair => MoveFromCacheToLong(pair.Key)).ToArray());
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
                foreach (var keyValuePair in timeToLive)
                {
                    timeToLive[keyValuePair.Key]--;
                    if (keyValuePair.Value > 1) 
                        continue;
                    
                    timeToLive.Remove(keyValuePair.Key);
                    tasks.Add(MoveFromCacheToLong(keyValuePair.Key));
                }

                Task.WaitAll(tasks.ToArray());
            }
        }

        private async Task MoveFromCacheToLong(TKey key)
        {
            log.Debug($"key:{key.ToString()} data is expired");
            var value = await cacheStorage.GetAsync(key).ConfigureAwait(false);
            await longStorage.CreateOrUpdateAsync(key, value.Value).ConfigureAwait(false);
        }
    }
}