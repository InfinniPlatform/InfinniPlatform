using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Core.Security
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