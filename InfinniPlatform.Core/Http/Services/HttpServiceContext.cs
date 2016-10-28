using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Core.Http.Services
{
    internal class HttpServiceContext : IHttpServiceContext
    {
        public IHttpRequest Request { get; set; }
    }
}