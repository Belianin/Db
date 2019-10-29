using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Db.Logging.Abstractions;
using Db.Storage.Serialization;
using Db.Utils;

namespace Db.Storage.Keyless
{
    public class InFileKeylessStorage<TValue> : IKeylessStorage<TValue>
    {
        private readonly ISerialization serialization = new JsonSerialization();

        private readonly ILog log;

        private readonly string fileName;

        public InFileKeylessStorage(string name, ILog log)
        {
            fileName = name;
            this.log = log.WithPrefix($"{typeof(TValue).Name} FILE-STORAGE");
            
            EnsureFileExistence();
        }

        public IEnumerable<TValue> Get(Func<TValue, bool> selector)
        {
            EnsureFileExistence();
            var result = File.ReadLines(fileName)
                .Select(line => serialization.Deserialize<TValue>(line))
                .Where(selector);
            //log.Debug($"Got {result.Count()} elements");

            return result;
        }

        public void Create(TValue value)
        {
            EnsureFileExistence();
            using (var writer = File.AppendText(fileName))
            {
                writer.Write($"{serialization.Serialize(value)}\n");
            }
        }

        public void Create(IEnumerable<TValue> value)
        {
            EnsureFileExistence();
            using (var writer = File.AppendText(fileName))
            {
                writer.Write($"{string.Join("\n", value.Select(serialization.Serialize))}\n");
            }
        }

        public void Update(Func<TValue, bool> selector, TValue value)
        {
            EnsureFileExistence();
            var lines = File.ReadLines(fileName)
                .Select(l => selector(serialization.Deserialize<TValue>(l)) ? serialization.Serialize(value) : l)
                .ToArray();
            
            File.WriteAllLines(fileName, lines);
        }

        public void Update(Func<TValue, bool> selector, Action<TValue> action)
        {
            EnsureFileExistence();
            var lines = File.ReadLines(fileName)
                .Select(serialization.Deserialize<TValue>)
                .ForEach(v =>
                {
                    if (selector(v)) 
                        action(v);
                })
                .Select(serialization.Serialize)
                .ToArray();
            
            File.WriteAllLines(fileName, lines);
        }

        public void Delete(Func<TValue, bool> selector)
        {
            EnsureFileExistence();
            var lines = File.ReadLines(fileName)
                .Where(l => !selector(serialization.Deserialize<TValue>(l)))
                .ToArray();
            
            File.WriteAllLines(fileName, lines);
        }

        public void Clear()
        {
            File.Create(fileName);
        }

        private void EnsureFileExistence()
        {
            if (!File.Exists(fileName))
            {
                log.Warn($"File \"{fileName}\" for storage of {typeof(TValue).Name} is missing");
                log.Info("Creating an empty file");
                File.Create(fileName);
            }
        }
    }
}