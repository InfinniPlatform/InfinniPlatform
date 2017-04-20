using System.Threading.Tasks;

using InfinniPlatform.Diagnostics;
using InfinniPlatform.Http;

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

        public async Task<object> GetStatus(IHttpRequest request)
        {
            return await _connectionFactory.RedisClient.GetStatusAsync();
        }
    }
}