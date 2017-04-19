namespace InfinniPlatform.Cache
{
    internal static class CachingHelpers
    {
        public const string RedisStarWildcards = "*";

        // IPerformanceLog
        public const string PerformanceLogRedisContainsMethod = "Contains";
        public const string PerformanceLogRedisGetMethod = "Get";
        public const string PerformanceLogRedisSetMethod = "Set";
        public const string PerformanceLogRedisRemoveMethod = "Remove";
        public const string PerformanceLogRedisClearMethod = "Clear";
        public const string PerformanceLogRedisSubscribeMethod = "Subscribe";
        public const string PerformanceLogRedisPublishMethod = "Publish";
        public const string PerformanceLogRedisHandleMethod = "Handle";


        /// <summary>
        /// Возвращает ключ кэширования с указанием пространства имен.
        /// </summary>
        /// <param name="unwrappedKey">Ключ кэширования без указания пространства имен.</param>
        /// <param name="name">Пространство имен.</param>
        /// <returns>Ключ кэширования с указанием пространства имен.</returns>
        public static string WrapCacheKey(this string unwrappedKey, string name)
        {
            return $"{name}.{unwrappedKey}";
        }

        /// <summary>
        /// Возвращает ключ кэширования без указания пространства имен.
        /// </summary>
        /// <param name="wrappedKey">Ключ кэширования с указанием пространства имен.</param>
        /// <param name="name">Пространство имен.</param>
        /// <returns>Ключ кэширования без указания пространства имен.</returns>
        public static string UnwrapCacheKey(this string wrappedKey, string name)
        {
            string unwrappedKey = null;

            if (!string.IsNullOrEmpty(name))
            {
                if (!string.IsNullOrEmpty(wrappedKey) && wrappedKey.StartsWith(name + "."))
                {
                    unwrappedKey = wrappedKey.Substring(name.Length + 1);
                }
            }
            else
            {
                unwrappedKey = wrappedKey;
            }

            return unwrappedKey;
        }
    }
}