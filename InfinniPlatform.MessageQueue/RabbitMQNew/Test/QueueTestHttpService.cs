using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.MessageQueue.RabbitMQNew.Test
{
    public class QueueTestHttpService : IHttpService
    {
        public QueueTestHttpService(IProducer producer, IQueningConsumer consumer)
        {
            _producer = producer;
            _consumer = consumer;
        }

        private readonly IQueningConsumer _consumer;
        private readonly IProducer _producer;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/que"] = Process;
        }

        private Task<object> Process(IHttpRequest request)
        {
            _producer.Produce("Success.");

            Thread.Sleep(2000);

            var message = _consumer.Consume();

            return Task.FromResult<object>(new
                                           {
                                               IsValid = true,
                                               Message = message
                                           });
        }
    }
}