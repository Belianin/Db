using System;
using System.Collections.Generic;
using System.Linq;
using Db.Utils;

namespace Db.Storage.Keyless
{
    [Obsolete("Useless")]
    public class InMemoryKeylessStorage<TValue> : IKeylessStorage<TValue>
    {
        private List<TValue> list = new List<TValue>();

        public IEnumerable<TValue> Get(Func<TValue, bool> selector)
        {
            return list.Where(selector);
        }
        
        public void Create(IEnumerable<TValue> values)
        {
            foreach (var value in values)
            {
                list.Add(value);
            }
        }

        public void Update(Func<TValue, bool> selector, TValue value)
        {
            list = list.Select(e => selector(e) ? value : e).ToList();
        }

        public void Update(Func<TValue, bool> selector, Action<TValue> action)
        {
            list.Where(selector).ForEach(action);
        }

        public void Delete(Func<TValue, bool> selector)
        {
            list = list.Where(e => !selector(e)).ToList();
        }

        public static implicit operator InMemoryKeylessStorage<TValue>(List<TValue> list)
        {
            return new InMemoryKeylessStorage<TValue>{list = list};
        }
        
        public static implicit operator List<TValue>(InMemoryKeylessStorage<TValue> storage)
        {
            return storage.list;
        }
    }
}