using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.Http.Services
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