using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.MessageQueue.RabbitMQNew.Test
{
    public class QueueTestHttpService : IHttpService
    {
        public QueueTestHttpService(IProducer producer, IQueningConsumer queningConsumer, IEventingConsumer eventingConsumer)
        {
            _producer = producer;
            _queningConsumer = queningConsumer;
            _eventingConsumer = eventingConsumer;
        }

        private readonly IEventingConsumer _eventingConsumer;
        private readonly IProducer _producer;

        private readonly IQueningConsumer _queningConsumer;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/produce"] = Produce;
            builder.Get["/consume"] = Consume;
            builder.Get["/register"] = RegisterEvent;
        }

        private Task<object> Produce(IHttpRequest request)
        {
            _producer.Produce("Success.");

            return Task.FromResult<object>(new
                                           {
                                               IsValid = true,
                                               Message = "Message produced."
                                           });
        }

        private Task<object> Consume(IHttpRequest request)
        {
            var message = _queningConsumer.Consume();

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
            _eventingConsumer.AddRecievedEvent((sender, args) => { Console.WriteLine(args.Body); });

            return Task.FromResult<object>(new
                                           {
                                               IsValid = true,
                                               Message = "Event handler registered."
                                           });
        }
    }
}