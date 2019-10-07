using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Db.Logging;
using Db.Logging.Abstractions;
using Db.Storage.Serialization;
using Db.Utils;

namespace Db.Storage
{
    [Obsolete("Держать лишь один дескриптор всегда + enable key in memory - delete-update")]
    public class OneFileStorage<TValue>
    {
        private readonly ISerialization serialization = new JsonSerialization();

        private readonly ILog log;

        private readonly string fileName;

        public OneFileStorage(string name, ILog log = null)
        {
            this.fileName = name;
            this.log = log == null ? new FakeLog() : log.WithPrefix($"OneFileStorage");
        }
        
        public async Task<Result> CreateAsync(TValue value)
        {
            using (var writer = File.AppendText(fileName))
            {
                await writer.WriteAsync(serialization.Serialize(value)).ConfigureAwait(false);
            }
            
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Func<TValue, bool> keySelector, TValue value)
        {
            var lines = File.ReadLines(fileName)
                .Select(l => keySelector(serialization.Deserialize<TValue>(l)) ? serialization.Serialize(value) : l)
                .ToArray();
            File.WriteAllLines(fileName, lines);

            return Result.Ok();
        }

        public async Task<Result<TValue>> GetAsync(Func<TValue, bool> keySelector)
        {
            foreach (var line in File.ReadLines(fileName))
            {
                var value = serialization.Deserialize<TValue>(line);
                if (keySelector(value))
                    return value;
            }

            return "No value";
        }

        public async Task<Result<IEnumerable<TValue>>> GetAllAsync(Func<TValue, bool> keySelector)
        {
            var result = new List<TValue>();
            foreach (var line in File.ReadLines(fileName))
            {
                var value = serialization.Deserialize<TValue>(line);
                if (keySelector(value))
                    result.Add(value);
            }

            return result;
        }

        public async Task<Result> DeleteAsync(Func<TValue, bool> keySelector)
        {
            var lines = File.ReadLines(fileName)
                .Where(l => !keySelector(serialization.Deserialize<TValue>(l)))
                .ToArray();
            File.WriteAllLines(fileName, lines);

            return Result.Ok();
        }
    }
}