using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Queues.Producers;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;

namespace InfinniPlatform.PushNotification.MessageBus
{
    /// <summary>
    /// Масштабирующая шина сообщений для SignalR на базе RabbitMQ.
    /// </summary>
    /// <remarks>
    /// Ипользуется для распределения нагрузки на SignalR при работе в кластере.
    /// </remarks>
    public class SignalRMessageBus : ScaleoutMessageBus, IBroadcastConsumer
    {
        public const int DefaultStreamIndex = 0;
        private static long _payloadId;

        public SignalRMessageBus(IDependencyResolver resolver,
                                 ScaleoutConfiguration configuration,
                                 IBroadcastProducer producer)
            : base(resolver, configuration)
        {
            _producer = producer;

            Open(DefaultStreamIndex);
        }

        private readonly IBroadcastProducer _producer;

        public Type MessageType => typeof(ScaleoutMessageWrapper);

        public Task Consume(IMessage message)
        {
            return Task.Run(() =>
                            {
                                var rabbitMessage = (ScaleoutMessageWrapper)message.GetBody();

                                OnReceived(rabbitMessage.StreamIndex, 0, ScaleoutMessage.FromBytes(rabbitMessage.Body));
                            });
        }

        public Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(true);
        }

        protected override async Task Send(IList<Message> messages)
        {
            var scaleoutMessageWrapper = new ScaleoutMessageWrapper
                                         {
                                             Body = new ScaleoutMessage(messages).ToBytes(),
                                             StreamIndex = DefaultStreamIndex
                                         };

            await _producer.PublishAsync(scaleoutMessageWrapper);
        }

        protected override async Task Send(int streamIndex, IList<Message> messages)
        {
            var scaleoutMessageWrapper = new ScaleoutMessageWrapper
                                         {
                                             Body = new ScaleoutMessage(messages).ToBytes(),
                                             StreamIndex = streamIndex
                                         };

            await _producer.PublishAsync(scaleoutMessageWrapper);
        }

        protected override void OnReceived(int streamIndex, ulong id, ScaleoutMessage message)
        {
            base.OnReceived(streamIndex, (ulong)Interlocked.Increment(ref _payloadId), message);
        }
    }
}