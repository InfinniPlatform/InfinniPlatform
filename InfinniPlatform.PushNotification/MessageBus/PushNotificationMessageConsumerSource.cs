using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.PushNotification.MessageBus
{
    /// <summary>
    /// Источник зарегистрированных потребителей сообщений.
    /// </summary>
    public class PushNotificationMessageConsumerSource : IMessageConsumerSource
    {
        public PushNotificationMessageConsumerSource(IAppEnvironment appEnvironment,
                                                     SignalRMessageBus signalRMessageBus)
        {
            _appEnvironment = appEnvironment;
            _signalRMessageBus = signalRMessageBus;
        }

        private readonly IAppEnvironment _appEnvironment;
        private readonly SignalRMessageBus _signalRMessageBus;

        public IEnumerable<IConsumer> GetConsumers()
        {
            return _appEnvironment.IsInCluster
                       ? new[] { _signalRMessageBus }
                       : Enumerable.Empty<IConsumer>();
        }
    }
}