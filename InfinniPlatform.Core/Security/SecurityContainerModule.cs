using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.Core.Abstractions.Security;

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