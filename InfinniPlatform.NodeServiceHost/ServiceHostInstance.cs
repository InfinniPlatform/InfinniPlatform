using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Owin.Modules;

namespace InfinniPlatform.NodeServiceHost
{
    public class ServiceHostInstance
    {
        public ServiceHostInstance(IHostingService hostingService, IOwinHostingContext owinHostingContext)
        {
            HostingService = hostingService;
            OwinHostingContext = owinHostingContext;
        }

        public IHostingService HostingService { get; }

        public IOwinHostingContext OwinHostingContext { get; }
    }
}