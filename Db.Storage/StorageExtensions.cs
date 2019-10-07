using System.Threading.Tasks;
using Db.Logging;
using Db.Logging.Abstractions;
using Db.Utils;

namespace Db.Storage
{
    public static class StorageExtensions
    {
        public static async Task<Result<TValue>> GetOrCreateAsync<TValue, TKey>(this IStorage<TKey> storage, TKey key)
            where TValue : new()
        {
            var getResult = await storage.GetAsync<TValue>(key).ConfigureAwait(false);
            if (getResult.IsSuccess)
            {
                return getResult;
            }

            await storage.CreateOrUpdateAsync(key, new TValue()).ConfigureAwait(false);
            return await storage.GetAsync<TValue>(key).ConfigureAwait(false);
        }

        public static async Task<Result<TValue>> GetOrCreateAsync<TValue, TKey>(this IStorage<TKey> storage, TKey key,
            TValue value)
        {
            var getResult = await storage.GetAsync<TValue>(key).ConfigureAwait(false);
            if (getResult.IsSuccess)
            {
                return getResult;
            }

            await storage.CreateOrUpdateAsync(key, value).ConfigureAwait(false);
            return await storage.GetAsync<TValue>(key).ConfigureAwait(false);
        }

        public static async Task<Result<TValue>> GetOrCreateAsync<TKey, TValue>(this IStorage<TKey, TValue> storage,
            TKey key) where TValue : new()
        {
            var getResult = await storage.GetAsync(key).ConfigureAwait(false);
            if (getResult.IsSuccess)
            {
                return getResult;
            }

            await storage.CreateOrUpdateAsync(key, new TValue()).ConfigureAwait(false);
            return await storage.GetAsync(key).ConfigureAwait(false);
        }

        public static async Task<Result<TValue>> GetOrCreateAsync<TKey, TValue>(this IStorage<TKey, TValue> storage,
            TKey key, TValue value)
        {
            var getResult = await storage.GetAsync(key).ConfigureAwait(false);
            if (getResult.IsSuccess)
            {
                return getResult;
            }

            await storage.CreateOrUpdateAsync(key, value).ConfigureAwait(false);
            return await storage.GetAsync(key).ConfigureAwait(false);
        }

        public static IStorage<TKey, TValue> WithCaching<TKey, TValue>(this IStorage<TKey, TValue> storage,
            IStorage<TKey, TValue> cachingStorage, ILog log = null)
        {
            return new CachingStorage<TKey, TValue>(cachingStorage, storage, log ?? new FakeLog());
        }
    }
}