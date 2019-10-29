using System.Collections.Generic;
using System.Threading;

namespace Db.Storage.Keyless
{
    public static class KeylessStorageExtensions
    {
        public static IEnumerable<T> GetAll<T>(this IKeylessStorage<T> storage)
        {
            return storage.Get(x => true);
        }
    }
}