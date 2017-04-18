using System.Threading.Tasks;
using InfinniPlatform.Core.Abstractions.Diagnostics;
using InfinniPlatform.Core.Abstractions.Dynamic;
using InfinniPlatform.Core.Abstractions.Http;

namespace InfinniPlatform.Caching.Redis.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии подсистемы кэширования данных.
    /// </summary>
    internal sealed class CachingStatusProvider : ISubsystemStatusProvider
    {
        public CachingStatusProvider(RedisConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private readonly RedisConnectionFactory _connectionFactory;
        
        public string Name => "caching";
        
        public async Task<object> GetStatus(IHttpRequest request)
        {
            return new DynamicWrapper { ["redis"] = await _connectionFactory.RedisClient.GetStatusAsync() };
        }
    }
}