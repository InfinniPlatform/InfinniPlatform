using System;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Agent.InfinniNode
{
    public class TaskOutputHandler : ITaskOutputHandler
    {
        private const int Capacity = 30;

        public TaskOutputHandler(IJsonObjectSerializer serializer)
        {
            _serializer = serializer;
            _httpClient = new HttpClient();
        }

        private readonly HttpClient _httpClient;
        private readonly IJsonObjectSerializer _serializer;

        public async Task Handle(NodeOutputEventArgs args)
        {
            if (!args.IsOutputClosed)
            {
                await SendOutput(args.Output);
            }
        }

        private async Task SendOutput(string log)
        {
            var address = "localhost";
            var port = 9901;
            var path = "taskStatus";

            var uriString = $"http://{address}:{port}/server/{path}";
            var memberValue = Guid.NewGuid().ToString("D");
            var convertToString = _serializer.ConvertToString(new DynamicWrapper
                                                              {
                                                                  { "TaskId", memberValue },
                                                                  { "Log", log }
                                                              });

            var requestContent = new StringContent(convertToString, _serializer.Encoding, HttpConstants.JsonContentType);
            await _httpClient.PostAsync(new Uri(uriString), requestContent);
        }
    }


    public interface ITaskOutputHandler
    {
        Task Handle(NodeOutputEventArgs args);
    }
}