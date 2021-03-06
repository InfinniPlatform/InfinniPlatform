﻿namespace InfinniPlatform.Cache
{
    internal static class CachingHelpers
    {
        public const string RedisStarWildcards = "*";

        // IPerformanceLogger
        public const string PerfLogRedisContainsMethod = "Contains";
        public const string PerfLogRedisGetMethod = "Get";
        public const string PerfLogRedisSetMethod = "Set";
        public const string PerfLogRedisRemoveMethod = "Remove";
        public const string PerfLogRedisClearMethod = "Clear";
        public const string PerfLogRedisSubscribeMethod = "Subscribe";
        public const string PerfLogRedisPublishMethod = "Publish";
        public const string PerfLogRedisHandleMethod = "Handle";


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