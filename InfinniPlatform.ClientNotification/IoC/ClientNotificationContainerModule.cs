using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Core.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.ClientNotification.IoC
{
    internal sealed class ClientNotificationContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<SignalRWebClientNotificationServiceFactory>()
                   .As<IWebClientNotificationServiceFactory>()
                   .SingleInstance();

            builder.RegisterType<WebClientNotificationComponent>()
                   .As<IWebClientNotificationComponent>()
                   .SingleInstance();
        }
    }
}