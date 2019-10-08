using System.Collections.Generic;

namespace Db.Storage.Keyless
{
    public static class KeylessStorageExtensions
    {
        public static void Create<T>(this IKeylessStorage<T> storage, T value)
        {
            storage.Create(new []{value});
        }
        
        public static InMemoryKeylessStorage<T> ToKeylessStorage<T>(this List<T> list)
        {
            return new InMemoryKeylessStorage<T>{List = list};
        }
    }
}