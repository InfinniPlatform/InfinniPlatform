using InfinniPlatform.Owin.Modules;
using InfinniPlatform.PushNotification.Owin;
using InfinniPlatform.PushNotification.SignalR;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.PushNotification;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace InfinniPlatform.PushNotification.IoC
{
    public class SignalRContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Список всех точек обмена SignalR

            builder.RegisterType<SignalRPushNotificationServiceHub>()
                   .AsSelf()
                   .As<IHub>()
                   .InstancePerDependency()
                   .ExternallyOwned();

            // Внутренние зависимости SignalR

            builder.RegisterType<SignalRDependencyResolver>()
                   .As<IDependencyResolver>()
                   .SingleInstance();

            builder.RegisterType<SignalRHubDescriptorProvider>()
                   .As<IHubDescriptorProvider>()
                   .SingleInstance();

            // Модуль OWIN для SignalR

            builder.RegisterType<SignalROwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            // Публичные контракты

            builder.RegisterType<SignalRPushNotificationService>()
                   .As<IPushNotificationService>()
                   .SingleInstance();
        }
    }
}