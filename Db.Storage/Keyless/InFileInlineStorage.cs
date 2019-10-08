using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Db.Storage.Serialization;
using Db.Utils;

namespace Db.Storage.Keyless
{
    public class InFileInlineStorage<TValue> : IKeylessStorage<TValue>
    {
        private readonly ISerialization serialization = new JsonSerialization();

        private readonly string fileName;

        public InFileInlineStorage(string name)
        {
            fileName = name;
        }

        public TValue Get(Func<TValue, bool> selector)
        {
            return File.ReadLines(fileName)
                .Select(serialization.Deserialize<TValue>)
                .FirstOrDefault(selector);
        }

        public IEnumerable<TValue> GetAll(Func<TValue, bool> selector)
        {
            return File.ReadLines(fileName)
                .Select(line => serialization.Deserialize<TValue>(line))
                .Where(selector);
        }

        public void Create(TValue value)
        {
            using (var writer = File.AppendText(fileName))
            {
                writer.Write(serialization.Serialize(value));
            }
        }

        public void Create(IEnumerable<TValue> value)
        {
            using (var writer = File.AppendText(fileName))
            {
                writer.Write(string.Join("\n", value.Select(serialization.Serialize)));
            }
        }

        public void Update(Func<TValue, bool> selector, TValue value)
        {
            var lines = File.ReadLines(fileName)
                .Select(l => selector(serialization.Deserialize<TValue>(l)) ? serialization.Serialize(value) : l)
                .ToArray();
            
            File.WriteAllLines(fileName, lines);
        }

        public void Delete(Func<TValue, bool> selector)
        {
            var lines = File.ReadLines(fileName)
                .Where(l => !selector(serialization.Deserialize<TValue>(l)))
                .ToArray();
            
            File.WriteAllLines(fileName, lines);
        }
    }
}