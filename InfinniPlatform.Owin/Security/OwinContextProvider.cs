using InfinniPlatform.Sdk.IoC;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Security
{
    internal sealed class OwinContextProvider : IOwinContextProvider
    {
        public OwinContextProvider(IContainerResolver containerResolver)
        {
            _containerResolver = containerResolver;
        }

        private readonly IContainerResolver _containerResolver;

        public IOwinContext GetOwinContext()
        {
            var owinContext = _containerResolver.ResolveOptional<IOwinContext>();

            return owinContext;
        }
    }
}