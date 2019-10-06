using System.Threading.Tasks;
using Db.Utils;

namespace Db.Storage
{
    public interface ICustomStorage
    {
        Task<Result> CreateOrUpdateAsync<T>(string key, T value);

        Task<Result<T>> GetAsync<T>(string key);

        Task<Result> DeleteAsync(string key);
    }
}