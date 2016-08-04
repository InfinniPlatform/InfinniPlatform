using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Owin.Services
{
    internal class HttpServiceContextProvider : IHttpServiceContextProvider
    {
        public HttpServiceContextProvider(IContainerResolver resolver)
        {
            _resolver = resolver;
        }


        private readonly IContainerResolver _resolver;


        public IHttpServiceContext GetContext()
        {
            return _resolver.Resolve<IHttpServiceContext>();
        }
    }
}