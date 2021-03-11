using System;

namespace SX.Common.Shared.Contracts
{
    public interface ICacheProvider
    {
        object this[string key] { get; }

        //bool Contains(string key);

        object Get(string key);
        T Get<T>(string key) where T : class;

        void Set<T>(string key, T value, TimeSpan timespan);
        void Set<T>(string key, T value);

        void Remove(string key);
    }
}
