using InfinniPlatform.IoC;

namespace InfinniPlatform.Auth.HttpService.IoC
{
    /// <summary>
    /// Dependency registration module for <see cref="InfinniPlatform.Auth.HttpService" />.
    /// </summary>
    public class AuthHttpServiceContainerModule : IContainerModule
    {
        private readonly AuthHttpServiceOptions _options;

        /// <summary>
        /// Creates new instance of <see cref="AuthHttpServiceContainerModule"/>.
        /// </summary>
        public AuthHttpServiceContainerModule(AuthHttpServiceOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            builder.RegisterType<UserEventHandlerInvoker>()
                   .AsSelf()
                   .SingleInstance();
        }
    }
}