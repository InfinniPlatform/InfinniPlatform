using System;
using System.Reflection;
using System.Threading.Tasks;
using InfinniPlatform.Cache;
using InfinniPlatform.Http;
using InfinniPlatform.IoC;

namespace InfinniPlatform.ServiceHost
{
    public class ContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Register dependencies
            builder.RegisterHttpServices(GetType().GetTypeInfo().Assembly);
        }
    }

    public class HttpService: IHttpService
    {
        private readonly ISharedCache _sharedCache;

        public HttpService(ISharedCache sharedCache)
        {
            _sharedCache = sharedCache;
        }

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/cache"]=Func;
        }

        private Task<object> Func(IHttpRequest httpRequest)
        {
            _sharedCache.Set("123", "123");
            return Task.FromResult<object>(true);
        }
    }
}