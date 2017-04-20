using System.Threading.Tasks;

using InfinniPlatform.Diagnostics;
using InfinniPlatform.Http;
using InfinniPlatform.MessageQueue.Management;

namespace InfinniPlatform.MessageQueue.Diagnostics
{
    internal class RabbitMqMessageQueueStatusProvider : ISubsystemStatusProvider
    {
        public RabbitMqMessageQueueStatusProvider(RabbitMqManagementHttpClient client)
        {
            _client = client;
        }

        private readonly RabbitMqManagementHttpClient _client;

        public string Name => RabbitMqMessageQueueOptions.SectionName;

        public async Task<object> GetStatus(IHttpRequest request)
        {
            return await _client.GetOverview();
        }
    }
}