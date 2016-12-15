using InfinniPlatform.Caching.Contract;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Caching
{
    public static class CachingExtensions
    {
        public static object GetObject(this ICache cache, string key)
        {
            object value;

            TryGetObject(cache, key, out value);

            return value;
        }

        public static bool TryGetObject(this ICache cache, string key, out object value)
        {
            string stringValue;

            if (cache.TryGet(key, out stringValue))
            {
                value = JToken.Parse(stringValue);
                return true;
            }

            value = null;
            return false;
        }

        public static void SetObject(this ICache cache, string key, object value)
        {
            var stringValue = JsonConvert.SerializeObject(value);

            cache.Set(key, stringValue);
        }
    }
}