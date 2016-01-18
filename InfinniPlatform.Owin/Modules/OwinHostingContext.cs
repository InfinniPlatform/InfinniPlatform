using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.RestApi;

namespace InfinniPlatform.Owin.Modules
{
    public sealed class OwinHostingContext : IOwinHostingContext
    {
        public OwinHostingContext(HostingConfig configuration, IContainerResolver containerResolver, IOwinMiddlewareResolver owinMiddlewareResolver)
        {
            Configuration = configuration;
            ContainerResolver = containerResolver;
            OwinMiddlewareResolver = owinMiddlewareResolver;
        }

        public HostingConfig Configuration { get; }

        public IContainerResolver ContainerResolver { get; }

        public IOwinMiddlewareResolver OwinMiddlewareResolver { get; }
    }
}