using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq.Hosting
{
    public class MessageConsumeHandler : IMessageConsumeHandler
    {
        public MessageConsumeHandler(IBasicPropertiesProvider basicPropertiesProvider)
        {
            _basicPropertiesProvider = basicPropertiesProvider;
        }

        private readonly IBasicPropertiesProvider _basicPropertiesProvider;

        public Task OnBefore(IMessage message)
        {
            return Task.Run(() =>
                            {
                                var dictionary = _basicPropertiesProvider.GetHeaders(message);

                                //TODO Добавить возможность установки tenantId в текущий контекст.
                                if (dictionary.ContainsKey(MessageHeadersTypes.TenantId))
                                {
                                    var tenantId = dictionary[MessageHeadersTypes.TenantId].Invoke();
                                }
                            });
        }

        public Task OnAfter(IMessage message)
        {
            return Task.FromResult<object>(null);
        }

        public Task<bool> OnError(IMessage message, Exception error)
        {
            return Task.FromResult(true);
        }
    }
}