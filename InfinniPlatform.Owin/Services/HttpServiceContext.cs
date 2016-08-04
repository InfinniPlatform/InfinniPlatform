using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Owin.Services
{
    internal class HttpServiceContext : IHttpServiceContext
    {
        public IHttpRequest Request { get; set; }
    }
}