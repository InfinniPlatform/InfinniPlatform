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
        private readonly RabbitMqMessageQueueOptions _rabbitMqMessageQueueOptions;

        public RabbitMqMessageQueueStatusProvider(RabbitMqManagementHttpClient client, RabbitMqMessageQueueOptions rabbitMqMessageQueueOptions)
        {
            _client = client;
            _rabbitMqMessageQueueOptions = rabbitMqMessageQueueOptions;
        }

        public string Name => _rabbitMqMessageQueueOptions.SectionName;

        public async Task<object> GetStatus(HttpRequest request)
        {
            return await _client.GetOverview();
        }
    }
}