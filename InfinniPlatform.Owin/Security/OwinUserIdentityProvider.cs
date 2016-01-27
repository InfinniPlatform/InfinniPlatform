using System.Security.Principal;

using InfinniPlatform.Sdk.Security;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Security
{
    internal sealed class OwinUserIdentityProvider : IUserIdentityProvider
    {
        public OwinUserIdentityProvider(IOwinContextProvider owinContextProvider)
        {
            _owinContextProvider = owinContextProvider;
        }

        private readonly IOwinContextProvider _owinContextProvider;

        public IIdentity GetCurrentUserIdentity()
        {
            var owinContext = _owinContextProvider.GetOwinContext();

            return GetUserIdentity(owinContext);
        }

        public static IIdentity GetUserIdentity(IOwinContext owinContext)
        {
            return (owinContext != null && owinContext.Request != null && owinContext.Request.User != null)
                ? owinContext.Request.User.Identity
                : null;
        }
    }
}