using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Authentication.Tests
{
    public class CacheHttpServiceOne : IHttpService
    {
        public CacheHttpServiceOne(IMemoryCache memoryCache,
                                   ISharedCache sharedCache,
                                   ITwoLayerCache twoLayerCache)
        {
            _memoryCache = memoryCache;
            _sharedCache = sharedCache;
            _twoLayerCache = twoLayerCache;
        }

        private readonly IMemoryCache _memoryCache;
        private readonly ISharedCache _sharedCache;
        private readonly ITwoLayerCache _twoLayerCache;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/cacheo";
            builder.Get["/set"] = Set;
        }

        private Task<object> Set(IHttpRequest httpRequest)
        {
            _memoryCache.Clear();
            _memoryCache.Set("memory", "memory");
            _sharedCache.Clear();
            _sharedCache.Set("shared", "shared");
            _twoLayerCache.Clear();
            _twoLayerCache.Set("twoLayer", "twoLayer");

            return Task.FromResult<object>("Ok.");
        }
    }


    public class CacheHttpServiceTwo : IHttpService
    {
        public CacheHttpServiceTwo(IMemoryCache memoryCache,
                                   ISharedCache sharedCache,
                                   ITwoLayerCache twoLayerCache)
        {
            _memoryCache = memoryCache;
            _sharedCache = sharedCache;
            _twoLayerCache = twoLayerCache;
        }

        private readonly IMemoryCache _memoryCache;
        private readonly ISharedCache _sharedCache;
        private readonly ITwoLayerCache _twoLayerCache;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/cachet";
            builder.Get["/get"] = Get;
        }

        private Task<object> Get(IHttpRequest httpRequest)
        {
            var s = _memoryCache.Get("memory");
            var s1 = _sharedCache.Get("shared");
            var s2 = _twoLayerCache.Get("twoLayer");

            return Task.FromResult<object>($"memory: {s}{Environment.NewLine}shared: {s1}{Environment.NewLine}twoLayer: {s2}");
        }
    }
}