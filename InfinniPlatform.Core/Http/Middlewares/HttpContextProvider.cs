using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.Http.Middlewares;

using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Core.Http.Middlewares
{
    internal class HttpContextProvider : IHttpContextProvider
    {
        public HttpContextProvider(IContainerResolver containerResolver)
        {
            _containerResolver = containerResolver;
        }


        private readonly IContainerResolver _containerResolver;


        public HttpContext GetHttpContext()
        {
            var accessor = _containerResolver.ResolveOptional<IHttpContextAccessor>();

            return accessor.HttpContext;
        }
    }
}