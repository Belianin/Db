using System.Collections.Generic;
using System.Threading;

namespace Db.Storage.Keyless
{
    public static class KeylessStorageExtensions
    {
        public static void Create<T>(this IKeylessStorage<T> storage, T value)
        {
            storage.Create(new []{value});
        }

        public static IEnumerable<T> GetAll<T>(this IKeylessStorage<T> storage)
        {
            return storage.Get(x => true);
        }
    }
}