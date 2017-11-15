using System.Threading.Tasks;
using InfinniPlatform.Diagnostics;
using InfinniPlatform.Http;
using InfinniPlatform.MessageQueue.Management;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.MessageQueue.Diagnostics
{
    internal class RabbitMqMessageQueueStatusProvider : ISubsystemStatusProvider
    {
        private readonly RabbitMqManagementHttpClient _client;

        public RabbitMqMessageQueueStatusProvider(RabbitMqManagementHttpClient client)
        {
            _client = client;
        }

        public string Name => RabbitMqMessageQueueOptions.SectionName;

        public async Task<object> GetStatus(HttpRequest request)
        {
            return await _client.GetOverview();
        }
    }
}