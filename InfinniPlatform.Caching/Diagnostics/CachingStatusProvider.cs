using System.Threading.Tasks;

using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Caching.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии подсистемы кэширования данных.
    /// </summary>
    internal sealed class CachingStatusProvider : ISubsystemStatusProvider
    {
        public CachingStatusProvider(CacheSettings cacheSettings, RedisConnectionFactory connectionFactory)
        {
            _cacheSettings = cacheSettings;
            _connectionFactory = connectionFactory;
        }


        private readonly CacheSettings _cacheSettings;
        private readonly RedisConnectionFactory _connectionFactory;


        public string Name => "caching";


        public async Task<object> GetStatus()
        {
            var status = new DynamicWrapper
                         {
                             { "type", _cacheSettings.Type }
                         };

            if (_cacheSettings.Type == CacheSettings.SharedCacheKey)
            {
                using (var client = _connectionFactory.GetClient())
                {
                    status["redis"] = await client.GetStatusAsync();
                }
            }

            return status;
        }
    }
}