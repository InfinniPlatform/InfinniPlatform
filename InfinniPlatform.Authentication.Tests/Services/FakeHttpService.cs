using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Queues.Producers;
using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Authentication.Tests.Services
{
    internal sealed class FakeHttpService : IHttpService
    {
        public FakeHttpService(IAppUserManager userManager, ITaskProducer producer)
        {
            _userManager = userManager;
            _producer = producer;
        }

        private readonly IAppUserManager _userManager;
        private readonly ITaskProducer _producer;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Fake";
            builder.Get["/SomeGet"] = request => Task.FromResult<object>((request.User != null) ? request.User.Name : null);
            builder.Post["/SomePost"] = request => Task.FromResult<object>((request.User != null) ? request.User.Name : null);
            builder.Post["/CreateUser"] = request => { _userManager.CreateUser(request.Form.UserName, request.Form.Password); return Task.FromResult<object>(null); };
            builder.Post["/FindUser"] = request => { _userManager.FindUserByName(request.Form.UserName); return Task.FromResult<object>(null); };
            builder.Post["/FindUserAsync"] = async request => await _userManager.FindUserByNameAsync(request.Form.UserName);
            builder.Get["/Pub"] = Func;
        }

        private async Task<object> Func(IHttpRequest httpRequest)
        {
            var messageBody = DateTime.Now.ToString("s");

            await _producer.PublishAsync(messageBody,"Q");

            return messageBody;
        }
    }


    [QueueName("Q")]
    public class Consumero : TaskConsumerBase<string>
    {
        protected override async Task Consume(Message<string> message)
        {
            await Task.Delay(500);
        }

        protected override Task<bool> OnError(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}