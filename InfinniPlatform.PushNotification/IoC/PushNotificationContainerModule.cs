using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.PushNotification.Contract;
using InfinniPlatform.PushNotification.MessageBus;
using InfinniPlatform.PushNotification.Owin;
using InfinniPlatform.PushNotification.SignalR;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.Sdk.Settings;

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
            // Список всех точек обмена SignalR

            builder.RegisterType<SignalRPushNotificationServiceHub>()
                   .AsSelf()
                   .As<IHub>()
                   .InstancePerDependency();

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
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<Microsoft.AspNet.SignalR.Messaging.MessageBus>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterFactory(CreateMessageBus)
                   .As<IMessageBus>()
                   .SingleInstance();

            builder.RegisterType<ScaleoutConfiguration>()
                   .As<ScaleoutConfiguration>()
                   .SingleInstance();

            builder.RegisterType<PushNotificationMessageConsumerSource>()
                   .As<IMessageConsumerSource>()
                   .SingleInstance();
        }

        private static IMessageBus CreateMessageBus(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppEnvironment>().IsInCluster
                       ? resolver.Resolve<SignalRMessageBus>()
                       : resolver.Resolve<Microsoft.AspNet.SignalR.Messaging.MessageBus>();
        }
    }
}