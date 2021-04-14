using Microsoft.Extensions.Caching.Memory;
using SX.Common.Shared.Contracts;
using System;

namespace SX.Common.Infrastructure.Services
{
    public class MemoryCacheProvider : ICacheProvider
    {
        protected MemoryCache _cache;

        protected const long CACHE_SIZE_LIMIT = 1024;
        protected const int TIMESPAN_IN_MINUTES = 1;


        public MemoryCacheProvider()
        {
            _cache = new MemoryCache(new MemoryCacheOptions
            {
                //SizeLimit = CACHE_SIZE_LIMIT
            });
        }


        public object this[string key]
        { get { return this.Get(key); } }

        //public bool Contains(string key)
        //{
        //    object result;
        //    return _cache.TryGetValue(key, out result);
        //}

        public object Get(string key)
        {
            //object result;
            return _cache.TryGetValue(key, out object result) ? result : null;
        }

        public T Get<T>(string key) where T : class
        {
            //T result;
            return _cache.TryGetValue<T>(key, out T result) ? result : default(T);
        }

        public void Set<T>(string key, T value, TimeSpan timespan)
        {
            _cache.Set<T>(key, value, timespan);
        }

        public void Set<T>(string key, T value) => this.Set<T>(key, value, new TimeSpan(0, TIMESPAN_IN_MINUTES, 0));

        public void Remove(string key) => _cache.Remove(key);
    }
}
