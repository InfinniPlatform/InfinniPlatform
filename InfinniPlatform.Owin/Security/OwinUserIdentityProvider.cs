using System.Security.Principal;

using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Security;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Security
{
    /// <summary>
    /// Предоставляет метод для получения идентификационных данных текущего пользователя.
    /// </summary>
    internal sealed class OwinUserIdentityProvider : IUserIdentityProvider
    {
        public OwinUserIdentityProvider(IContainerResolver containerResolver)
        {
            _containerResolver = containerResolver;
        }

        private readonly IContainerResolver _containerResolver;

        public IIdentity GetCurrentUserIdentity()
        {
            var owinContext = _containerResolver.ResolveOptional<IOwinContext>();

            return owinContext?.Request?.User?.Identity;
        }
    }
}