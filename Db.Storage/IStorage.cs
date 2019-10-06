using System.Threading.Tasks;
using Db.Utils;

namespace Db.Storage
{
    public interface IStorage<in TKey, TValue>
    {
        Task<Result> CreateOrUpdateAsync(TKey key, TValue value);

        Task<Result<TValue>> GetAsync(TKey key);

        Task<Result> DeleteAsync(TKey key);
    }
    
    public interface IStorage
    {
        Task<Result> CreateOrUpdateAsync<T>(string key, T value);

        Task<Result<T>> GetAsync<T>(string key);

        Task<Result> DeleteAsync(string key);
    }
}