using System;
using System.Collections.Generic;

namespace Db.Storage.Keyless
{
    public interface IKeylessStorage<TValue>
    {
        IEnumerable<TValue> Get(Func<TValue, bool> selector);
        
        void Create(TValue value);
        
        void Create(IEnumerable<TValue> values);

        void Update(Func<TValue, bool> selector, TValue value);

        void Update(Func<TValue, bool> selector, Action<TValue> action);

        void Delete(Func<TValue, bool> selector);

        void Clear();
    }
}