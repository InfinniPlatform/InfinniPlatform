using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Queues.Producers;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.MessageQueue
{
    public class Recconnect : IHttpService
    {
        public Recconnect(ITaskProducer taskProducer)
        {
            _taskProducer = taskProducer;
        }

        private readonly ITaskProducer _taskProducer;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "Reconnect";
            builder.Get["Send"] = Send;
        }

        private async Task<object> Send(IHttpRequest httpRequest)
        {
            for (var i = 0; i < 10000; i++)
            {
                await _taskProducer.PublishAsync(Guid.NewGuid().ToString("N"), "queue");
            }

            return "Ok.";
        }
    }


    [QueueName("queue")]
    public class ReconnectConsumer : TaskConsumerBase<string>
    {
        public ReconnectConsumer(ILog log)
        {
            _log = log;
        }

        private readonly ILog _log;

        protected override async Task Consume(Message<string> message)
        {
            await Task.Delay(5000);
            _log.Info(message.Body);
        }
    }
}