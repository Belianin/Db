using System.Threading.Tasks;
using Db.Utils;

namespace Db.Storage
{
    public static class CustomStorageExtensions
    {
        public static async Task<Result<T>> GetOrCreateAsync<T>(this ICustomStorage storage, string key) where T : new()
        {
            var getResult = await storage.GetAsync<T>(key).ConfigureAwait(false);
            if (getResult.IsSuccess)
            {
                return getResult;
            }
            
            await storage.CreateOrUpdateAsync(key, new T()).ConfigureAwait(false);
            return await storage.GetAsync<T>(key).ConfigureAwait(false);
        }
        
        public static async Task<Result<TValue>> GetOrCreateAsync<TKey, TValue>(this IStorage<TKey, TValue> storage, TKey key) where TValue : new()
        {
            var getResult = await storage.GetAsync(key).ConfigureAwait(false);
            if (getResult.IsSuccess)
            {
                return getResult;
            }

            await storage.CreateOrUpdateAsync(key, new TValue()).ConfigureAwait(false);
            return await storage.GetAsync(key).ConfigureAwait(false);
        }
        
        public static async Task<Result<TValue>> GetOrCreateAsync<TKey, TValue>(this IStorage<TKey, TValue> storage, TKey key, TValue value)
        {
            var getResult = await storage.GetAsync(key).ConfigureAwait(false);
            if (getResult.IsSuccess)
            {
                return getResult;
            }

            await storage.CreateOrUpdateAsync(key, value).ConfigureAwait(false);
            return await storage.GetAsync(key).ConfigureAwait(false);
        }
    }
}