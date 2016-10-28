using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq.Management.HttpAPI;
using InfinniPlatform.Sdk.Diagnostics;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.MessageQueue.Diagnostics
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