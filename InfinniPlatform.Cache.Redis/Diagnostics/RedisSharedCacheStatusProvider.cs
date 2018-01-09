using System.Threading.Tasks;

using InfinniPlatform.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Cache.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии подсистемы кэширования данных.
    /// </summary>
    internal class RedisSharedCacheStatusProvider : ISubsystemStatusProvider
    {
        public RedisSharedCacheStatusProvider(RedisConnectionFactory connectionFactory, RedisSharedCacheOptions redisSharedCacheOptions)
        {
            _connectionFactory = connectionFactory;
            _redisSharedCacheOptions = redisSharedCacheOptions;
        }

        private readonly RedisConnectionFactory _connectionFactory;
        private readonly RedisSharedCacheOptions _redisSharedCacheOptions;

        public string Name => _redisSharedCacheOptions.SectionName;

        public async Task<object> GetStatus(HttpRequest request)
        {
            return await _connectionFactory.RedisClient.Value.GetStatusAsync();
        }
    }
}