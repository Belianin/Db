using System;
using System.Collections.Generic;
using System.Linq;
using Db.Utils;

namespace Db.Storage.Keyless
{
    public class InMemoryKeylessStorage<TValue> : IKeylessStorage<TValue>
    {
        private List<TValue> list = new List<TValue>();

        public IEnumerable<TValue> Get(Func<TValue, bool> selector)
        {
            return list.Where(selector).ToList();
        }

        public void Create(TValue value)
        {
            list.Add(value);
        }

        public void Create(IEnumerable<TValue> values)
        {
            list.AddRange(values);
        }

        public void Update(Func<TValue, bool> selector, TValue value)
        {
            list = list.Select(e => selector(e) ? value : e).ToList();
        }

        public void Update(Func<TValue, bool> selector, Action<TValue> action)
        {
            list.Where(selector).ForEach(action); // return?
        }

        public void Delete(Func<TValue, bool> selector)
        {
            list = list.Where(e => !selector(e)).ToList();
        }

        public void Clear()
        {
            list.Clear();
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