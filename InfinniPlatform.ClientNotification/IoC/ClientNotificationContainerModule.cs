using InfinniPlatform.Factories;
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
        }
    }
}