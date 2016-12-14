using InfinniPlatform.Auth.Internal.Contract;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Auth.Internal.Security
{
    internal class SecurityContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<UserIdentityProvider>()
                   .As<IUserIdentityProvider>()
                   .SingleInstance();
        }
    }
}