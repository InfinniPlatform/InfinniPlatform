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
        public RedisSharedCacheStatusProvider(RedisConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private readonly RedisConnectionFactory _connectionFactory;

        public string Name => RedisSharedCacheOptions.SectionName;

        public async Task<object> GetStatus(HttpRequest request)
        {
            return await _connectionFactory.RedisClient.Value.GetStatusAsync();
        }
    }
}