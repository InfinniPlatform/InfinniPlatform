using System.Threading.Tasks;

using InfinniPlatform.Diagnostics;
using InfinniPlatform.Http;
using InfinniPlatform.MessageQueue.RabbitMq.Management.HttpAPI;

namespace InfinniPlatform.MessageQueue.RabbitMQ.Diagnostics
{
    internal sealed class MessageQueueStatusProvider : ISubsystemStatusProvider
    {
        public MessageQueueStatusProvider(RabbitMqManagementHttpClient client)
        {
            _client = client;
        }

        private readonly RabbitMqManagementHttpClient _client;

        public string Name => "messageQueue";

        public async Task<object> GetStatus(IHttpRequest request)
        {
            return await _client.GetOverview();
        }
    }
}