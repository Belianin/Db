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
}