using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Agent.Helpers
{
    /// <summary>
    /// Буфферизирует и отправляет InfinniPlatform.Server сообщения из вывода Infinni.Node.
    /// </summary>
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

        /// <summary>
        /// Отправляет сообщения из буффера и очищает его.
        /// </summary>
        /// <param name="serverAddress">Адрес InfinniPlatform.Server.</param>
        /// <param name="taskId">Идентификатор задачи.</param>
        public async Task Send(string serverAddress, string taskId)
        {
            var requestUri = new Uri($"http://{serverAddress}/server/taskStatus");

            var content = new DynamicWrapper
                          {
                              { "TaskId", taskId },
                              { "Log", _outputDataBuffer.ToArray() }
                          };

            var contentString = JsonObjectSerializer.Default.ConvertToString(content);

            var requestContent = new StringContent(contentString, JsonObjectSerializer.Default.Encoding, HttpConstants.JsonContentType);

            await _httpClient.PostAsync(requestUri, requestContent);

            _outputDataBuffer.Clear();
        }

        /// <summary>
        /// Добавляет строку в буфер стандартного вывода.
        /// </summary>
        /// <param name="output">Строка стандартного вывода.</param>
        public void Output(string output)
        {
            _outputDataBuffer.Add(output);
        }

        /// <summary>
        /// Добавляет строку в буфер вывода ошибок.
        /// </summary>
        /// <param name="error">Строка вывода ошибок.</param>
        public void Error(string error)
        {
            _errorDataBuffer.Add(error);
        }
    }
}