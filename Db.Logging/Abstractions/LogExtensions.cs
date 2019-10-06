namespace Db.Logging.Abstractions
{
    public static class LogExtensions
    {
        public static ILog WithPrefix(this ILog log, string prefix)
        {
            return new PrefixLog(log, prefix);
        }
    }
}