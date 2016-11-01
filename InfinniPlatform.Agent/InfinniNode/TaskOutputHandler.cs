using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
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
            _stringBuffer = new List<string>();
            _httpClient = new HttpClient();
        }

        private readonly HttpClient _httpClient;
        private readonly IJsonObjectSerializer _serializer;
        private List<string> _stringBuffer;

        public async Task Handle(NodeOutputEventArgs args)
        {
            if (!args.IsOutputClosed)
            {
                try
                {
                    if (_stringBuffer.Count < Capacity)
                    {
                        _stringBuffer.Add(args.Output);
                    }
                    else
                    {
                        await SendOutput(_stringBuffer);
                        ClearBuffer();
                    }
                }
                catch (Exception)
                {
                    //ignored
                }
            }
            else
            {
                try
                {
                    await SendOutput(_stringBuffer);
                    ClearBuffer();
                }
                catch (Exception)
                {
                    //ignored
                }
            }
        }

        private void ClearBuffer()
        {
            var newList = new List<string>();
            Interlocked.Exchange(ref _stringBuffer, newList);
        }

        private async Task SendOutput(List<string> stringBuffer)
        {
            var address = "localhost";
            var port = 9901;
            var path = "taskStatus";

            var log = stringBuffer.Aggregate<string, string>(null, (current, s) => current + $"{s}{Environment.NewLine}");

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