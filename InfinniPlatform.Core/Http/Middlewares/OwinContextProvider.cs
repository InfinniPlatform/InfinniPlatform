using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Core.Http.Middlewares
{
    internal class OwinContextProvider : IOwinContextProvider
    {
        public OwinContextProvider(IContainerResolver containerResolver)
        {
            _containerResolver = containerResolver;
        }


        private readonly IContainerResolver _containerResolver;


        public HttpContext GetOwinContext()
        {
            var owinContext = _containerResolver.ResolveOptional<HttpContext>();

            return owinContext;
        }
    }
}