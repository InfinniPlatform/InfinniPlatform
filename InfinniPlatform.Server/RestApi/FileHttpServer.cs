﻿using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
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

        private Task<object> GetConfigurationFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppFullName", (string)request.Query.AppFullName },
                                { "FileName", (string)request.Query.FileName }
                            };


            return WrapStreamResponse(_agentHttpClient.GetStream("config", address, port, arguments));
        }

        private async Task<object> SetConfigurationFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppFullName", (string)request.Query.AppFullName },
                                { "FileName", (string)request.Query.FileName },
                                { "Config", (string)request.Form.Config }
                            };

            return await _agentHttpClient.Post<ServiceResult<object>>("config", address, port, arguments);
        }

        private Task<object> GetAppLogFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppFullName", (string)request.Query.AppFullName }
                            };

            return WrapStreamResponse(_agentHttpClient.GetStream("appLog", address, port, arguments));
        }

        private Task<object> GetPerfLogFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppFullName", (string)request.Query.AppFullName }
                            };


            return WrapStreamResponse(_agentHttpClient.GetStream("perfLog", address, port, arguments));
        }

        private Task<object> GetNodeLogFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppFullName", (string)request.Query.AppFullName }
                            };

            return WrapStreamResponse(_agentHttpClient.GetStream("nodeLog", address, port, arguments));
        }

        private static Task<object> WrapStreamResponse(Task<Stream> stream)
        {
            return Task.FromResult<object>(new StreamHttpResponse(() => AsyncHelper.RunSync(() => stream)));
        }
    }
}