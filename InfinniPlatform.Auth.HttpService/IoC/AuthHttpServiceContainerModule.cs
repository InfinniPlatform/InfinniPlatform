using InfinniPlatform.IoC;

namespace InfinniPlatform.Auth.HttpService.IoC
{
    public class AuthHttpServiceContainerModule<TUser> : IContainerModule where TUser : AppUser
    {
        private readonly AuthHttpServiceOptions _options;

        public AuthHttpServiceContainerModule(AuthHttpServiceOptions options)
        {
            _options = options;
        }

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            builder.RegisterType<UserEventHandlerInvoker>()
                   .AsSelf()
                   .SingleInstance();
        }
    }
}