using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.MessageQueue.RabbitMQNew.Test
{
    public class QueueTestHttpService : IHttpService
    {
        public QueueTestHttpService(IProducer producer, IBasicConsumer basicConsumer, IEventingConsumer eventingConsumer)
        {
            _producer = producer;
            _basicConsumer = basicConsumer;
            _eventingConsumer = eventingConsumer;
        }

        private readonly IBasicConsumer _basicConsumer;

        private readonly IEventingConsumer _eventingConsumer;
        private readonly IProducer _producer;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/produce"] = Produce;
            builder.Get["/consume"] = Consume;
            builder.Get["/register"] = RegisterEvent;
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

        private Task<object> RegisterEvent(IHttpRequest request)
        {
            _eventingConsumer.AddRecievedEvent((sender, args) => { Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} EventingConsumer: {Encoding.UTF8.GetString(args.Body)}"); });

            return Task.FromResult<object>(new
                                           {
                                               IsValid = true,
                                               Message = "Event handler registered."
                                           });
        }
    }
}