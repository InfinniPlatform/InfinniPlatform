using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;

namespace Infinni.Server.Agent
{
    public class AgentHttpClient : IAgentHttpClient
    {
        public AgentHttpClient(IJsonObjectSerializer serializer)
        {
            _serializer = serializer;
            _httpClient = new HttpClient();
        }

        private readonly HttpClient _httpClient;
        private readonly IJsonObjectSerializer _serializer;

        public async Task<T> Get<T>(string path, string address, int port, DynamicWrapper queryContent = null)
        {
            var uriString = $"http://{address}:{port}/agent/{path}{ToQuery(queryContent)}";

            var response = await _httpClient.GetAsync(uriString);
            var content = await response.Content.ReadAsStreamAsync();
            var processResult = _serializer.Deserialize<T>(content);

            return processResult;
        }

        public async Task<T> Post<T>(string path, string address, int port, DynamicWrapper formContent)
        {
            var uriString = $"http://{address}:{port}/agent/{path}";

            var convertToString = _serializer.ConvertToString(formContent);
            var requestContent = new StringContent(convertToString, _serializer.Encoding, HttpConstants.JsonContentType);

            var response = await _httpClient.PostAsync(new Uri(uriString), requestContent);
            var content = await response.Content.ReadAsStreamAsync();
            var processResult = _serializer.Deserialize<T>(content);

            return processResult;
        }

        public async Task<Stream> GetStream(string path, string address, int port, DynamicWrapper queryContent = null)
        {
            var uriString = $"http://{address}:{port}/agent/{path}{ToQuery(queryContent)}";

            var response = await _httpClient.GetAsync(uriString);
            var content = await response.Content.ReadAsStreamAsync();

            return content;
        }

        private static string ToQuery(DynamicWrapper queryContent)
        {
            if (queryContent == null)
            {
                return null;
            }

            var query = "?";

            foreach (var pair in queryContent.ToDictionary())
            {
                query += $"{pair.Key}={pair.Value}&";
            }

            return query.TrimEnd('&');
        }
    }
}