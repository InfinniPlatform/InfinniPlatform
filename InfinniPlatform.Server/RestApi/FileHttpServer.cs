using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Server.Agent;

namespace InfinniPlatform.Server.RestApi
{
    /// <summary>
    /// Сервис взаимодействия с приложением Infinni.Agent.
    /// </summary>
    public class FileHttpService : IHttpService
    {
        public FileHttpService(IAgentHttpClient agentHttpClient)
        {
            _agentHttpClient = agentHttpClient;
        }

        private readonly IAgentHttpClient _agentHttpClient;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "server";

            builder.Get["/config"] = GetConfigurationFile;
            builder.Post["/config"] = SetConfigurationFile;

            builder.Get["/appLog"] = GetAppLogFile;
            builder.Get["/perfLog"] = GetPerfLogFile;
            builder.Get["/nodeLog"] = GetNodeLogFile;
        }

        private async Task<object> GetConfigurationFile(IHttpRequest request)
        {
            var serviceResult = new ServiceResult<string>();

            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppFullName", (string)request.Query.AppFullName },
                                { "FileName", (string)request.Query.FileName }
                            };

            using (var stream = await _agentHttpClient.GetStream("config", address, port, arguments))
            {
                serviceResult.Result = CleanUpJsonConfig(stream);
            }

            return serviceResult;
        }

        private async Task<object> SetConfigurationFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppFullName", (string)request.Query.AppFullName },
                                { "FileName", (string)request.Query.FileName },
                                { "Config", (string)request.Form.Config.ToString() }
                            };

            return await _agentHttpClient.Post<ServiceResult<object>>("config", address, port, arguments);
        }

        private async Task<object> GetAppLogFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppFullName", (string)request.Query.AppFullName }
                            };

            return await WrapLogResponse("appLog", address, port, arguments);
        }

        private async Task<object> GetPerfLogFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppFullName", (string)request.Query.AppFullName }
                            };

            return await WrapLogResponse("perfLog", address, port, arguments);
        }

        private async Task<object> GetNodeLogFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper();

            return await WrapLogResponse("nodeLog", address, port, arguments);
        }

        private async Task<object> WrapLogResponse(string command, string address, int port, DynamicWrapper arguments)
        {
            using (var stream = await _agentHttpClient.GetStream(command, address, port, arguments))
            {
                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(text))
                    {
                        text = "Log is empty.";
                    }

                    return new ServiceResult<string>
                           {
                               Success = true,
                               Result = text
                           };
                }
            }
        }

        private static Task<object> WrapStreamResponse(Task<Stream> stream)
        {
            return Task.FromResult<object>(new StreamHttpResponse(() => AsyncHelper.RunSync(() => stream)));
        }

        private static string CleanUpJsonConfig(Stream stream)
        {
            var configObject = JsonObjectSerializer.Formated.Deserialize(stream);
            var cleanConfigString = JsonObjectSerializer.Formated.ConvertToString(configObject);

            return cleanConfigString;
        }
    }
}