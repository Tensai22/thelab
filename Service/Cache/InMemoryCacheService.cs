namespace TheLab.Services.Cache
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        public object Get(string key)
        {
            _cache.TryGetValue(key, out var value);
            return value;
        }

        public void Set(string key, object value)
        {
            _cache[key] = value;
        }
    }
}
