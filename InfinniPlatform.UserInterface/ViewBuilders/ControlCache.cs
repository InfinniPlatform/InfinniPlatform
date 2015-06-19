using System;
using System.Runtime.Caching;

namespace InfinniPlatform.UserInterface.ViewBuilders
{
    internal static class ControlCache
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(5);
        private static readonly MemoryCache Cache = new MemoryCache("ControlCache");

        public static void Set(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (value != null)
            {
                Cache.Set(name, value, DateTimeOffset.Now.Add(Timeout));
            }
        }

        public static T Get<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            var valueObject = Cache.Get(name);

            return (valueObject is T) ? (T) valueObject : default(T);
        }
    }
}