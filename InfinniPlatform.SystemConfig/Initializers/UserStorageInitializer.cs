using InfinniPlatform.Api.Security;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Security;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.SystemConfig.Initializers
{
    public sealed class UserStorageInitializer : IStartupInitializer
    {
        private readonly IGlobalContext _globalContext;

        public UserStorageInitializer(IGlobalContext globalContext)
        {
            _globalContext = globalContext;
        }

        public void OnStart(HostingContextBuilder contextBuilder)
        {
            var applicationUserStore = ApplicationUserStorePersistentStorage.Instance;
            var applicationUserPasswordHasher = new DefaultApplicationUserPasswordHasher();

            contextBuilder.SetEnvironment<IApplicationUserStore>(applicationUserStore);
            contextBuilder.SetEnvironment<IApplicationUserPasswordHasher>(applicationUserPasswordHasher);
        }
    }
}