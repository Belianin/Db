using System;
using System.Collections;
using System.Collections.Generic;

namespace Db.Storage.Keyless
{
    public interface IKeylessStorage<TValue>
    {
        TValue Get(Func<TValue, bool> selector);

        IEnumerable<TValue> GetAll(Func<TValue, bool> selector);

        void Create(TValue value);

        void Create(IEnumerable<TValue> values);

        void Update(Func<TValue, bool> selector, TValue value);

        void Delete(Func<TValue, bool> selector);
    }
}