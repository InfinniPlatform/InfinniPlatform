using System.Reflection;

using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.PushNotification.MessageBus;
using InfinniPlatform.PushNotification.Owin;
using InfinniPlatform.PushNotification.SignalR;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.PushNotification;
using InfinniPlatform.Sdk.Queues;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Messaging;

namespace InfinniPlatform.PushNotification.IoC
{
    /// <summary>
    /// Модуль регистрации зависимостей <see cref="InfinniPlatform.PushNotification" />.
    /// </summary>
    public class PushNotificationContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            var assembly = typeof(PushNotificationContainerModule).GetTypeInfo().Assembly;

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

            builder.RegisterType<SignalRHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            // Публичные контракты

            builder.RegisterType<SignalRPushNotificationService>()
                   .As<IPushNotificationService>()
                   .SingleInstance();

            // Шина сообщений

            builder.RegisterType<SignalRMessageBus>()
                   .As<IMessageBus>()
                   .SingleInstance();

            builder.RegisterType<ScaleoutConfiguration>()
                   .As<ScaleoutConfiguration>()
                   .SingleInstance();

            builder.RegisterConsumers(assembly);
        }
    }
}