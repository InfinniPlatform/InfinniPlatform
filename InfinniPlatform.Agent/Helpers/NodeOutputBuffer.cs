using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Agent.Helpers
{
    public class NodeOutputBuffer
    {
        public NodeOutputBuffer()
        {
            _outputDataBuffer = new List<string>();
            _errorDataBuffer = new List<string>();
            _httpClient = new HttpClient();
        }

        private readonly List<string> _errorDataBuffer;
        private readonly HttpClient _httpClient;
        private readonly List<string> _outputDataBuffer;

        public int OutputCount => _outputDataBuffer.Count;

        public int ErrorCount => _errorDataBuffer.Count;

        public async Task Send(string taskId)
        {
            var address = "localhost";
            var port = 9901;
            var path = "taskStatus";

            var uriString = $"http://{address}:{port}/server/{path}";
            var convertToString = JsonObjectSerializer.Default.ConvertToString(new DynamicWrapper
                                                                               {
                                                                                   { "TaskId", taskId },
                                                                                   { "Log", _outputDataBuffer.ToArray() }
                                                                               });

            var requestContent = new StringContent(convertToString, JsonObjectSerializer.Default.Encoding, HttpConstants.JsonContentType);
            await _httpClient.PostAsync(new Uri(uriString), requestContent);
            _outputDataBuffer.Clear();
        }

        public void Output(string output)
        {
            _outputDataBuffer.Add(output);
        }

        public void Error(string error)
        {
            _errorDataBuffer.Add(error);
        }
    }
}