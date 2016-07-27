using System.Threading.Tasks;

using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Authentication.Tests
{
    public class CacheHttpService : IHttpService
    {
        public CacheHttpService(ICache cacheApi)
        {
            _cacheApi = cacheApi;
        }

        private readonly ICache _cacheApi;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Cache";
            builder.Post["/Set"] = Set;
            builder.Post["/Get"] = Get;
        }

        private Task<object> Set(IHttpRequest httpRequest)
        {
            var key = httpRequest.Form.Key as string;
            var value = httpRequest.Form.Value as string;

            _cacheApi.Set(key, value);

            return Task.FromResult<object>($"Set: {key} = {value}.");
        }

        private Task<object> Get(IHttpRequest httpRequest)
        {
            var key = httpRequest.Form.Key as string;
            var value = _cacheApi.Get(key);

            return Task.FromResult<object>($"Get: {value}.");
        }
    }
}