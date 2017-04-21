using InfinniPlatform.IoC;

namespace InfinniPlatform.Http
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