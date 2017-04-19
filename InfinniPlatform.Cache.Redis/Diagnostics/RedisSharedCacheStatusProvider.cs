using System.Threading.Tasks;

using InfinniPlatform.Core.Abstractions.Diagnostics;
using InfinniPlatform.Core.Abstractions.Http;

namespace InfinniPlatform.Cache.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии подсистемы кэширования данных.
    /// </summary>
    public class RedisSharedCacheStatusProvider : ISubsystemStatusProvider
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