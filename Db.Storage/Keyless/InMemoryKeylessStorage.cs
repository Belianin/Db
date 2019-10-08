using System;
using System.Collections.Generic;
using System.Linq;
using Db.Utils;

namespace Db.Storage.Keyless
{
    public class InMemoryKeylessStorage<TValue> : IKeylessStorage<TValue>
    {
        internal IList<TValue> List = new List<TValue>();

        public IEnumerable<TValue> Get(Func<TValue, bool> selector)
        {
            return List.Where(selector);
        }
        
        public void Create(IEnumerable<TValue> values)
        {
            foreach (var value in values)
            {
                List.Add(value);
            }
        }

        public void Update(Func<TValue, bool> selector, TValue value)
        {
            List = List.Select(e => selector(e) ? value : e).ToList();
        }

        public void Update(Func<TValue, bool> selector, Action<TValue> action)
        {
            List.Where(selector).ForEach(action);
        }

        public void Delete(Func<TValue, bool> selector)
        {
            List = List.Where(e => !selector(e)).ToList();
        }
    }
}