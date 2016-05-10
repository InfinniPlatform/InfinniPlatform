using System;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.MessageQueue.RabbitMQNew.Test
{
    public class QueueTestHttpService : IHttpService
    {
        public QueueTestHttpService(IProducer producer, IBasicConsumer basicConsumer)
        {
            _producer = producer;
            _basicConsumer = basicConsumer;
        }

        private readonly IBasicConsumer _basicConsumer;

        private readonly IProducer _producer;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/produce"] = Produce;
            builder.Get["/consume"] = Consume;
        }

        private Task<object> Produce(IHttpRequest request)
        {
            _producer.Publish(DateTime.Now.ToString("U"));

            return Task.FromResult<object>(new
                                           {
                                               IsValid = true,
                                               Message = "Message produced."
                                           });
        }

        private Task<object> Consume(IHttpRequest request)
        {
            var message = _basicConsumer.Get();

            if (message == null)
            {
                return Task.FromResult<object>(new
                                               {
                                                   IsValid = false,
                                                   ErrorMessage = "No new messages."
                                               });
            }

            return Task.FromResult<object>(new
                                           {
                                               IsValid = true,
                                               Message = message
                                           });
        }
    }


    public class TestConsumer : IEventingConsumer
    {
        public void Consume(byte[] messageBytes)
        {
            Console.WriteLine($"EventingConsumer1: {Encoding.UTF8.GetString(messageBytes)}");
        }
    }


    public class TestConsumer2 : IEventingConsumer
    {
        public void Consume(byte[] messageBytes)
        {
            Console.WriteLine($"EventingConsumer2: {Encoding.UTF8.GetString(messageBytes)}");
        }
    }
}