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
            builder.ServicePath = "/cache";
            builder.Post["/set"] = Set;
            builder.Post["/get"] = Get;
        }

        private Task<object> Set(IHttpRequest httpRequest)
        {
            var key = httpRequest.Form.Key.Value as string;
            var value = httpRequest.Form.Value.Value as string;

            _cacheApi.Set("twoLayer", value);

            return Task.FromResult<object>($"Set: {"twoLayer"} = {value}.");
        }

        private Task<object> Get(IHttpRequest httpRequest)
        {
            var s2 = _cacheApi.Get("twoLayer");

            return Task.FromResult<object>($"Get: {s2}.");
        }
    }
}