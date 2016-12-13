﻿using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using Infinni.Server.Agent;

namespace Infinni.Server.Tasks.Files
{
    public class ConfigSetTask : IServerTask
    {
        public ConfigSetTask(AgentHttpClient agentHttpClient)
        {
            _agentHttpClient = agentHttpClient;
        }

        private readonly AgentHttpClient _agentHttpClient;

        public string CommandName => "config";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public async Task<object> Run(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "FullName", (string)request.Query.FullName },
                                { "FileName", (string)request.Query.FileName },
                                { "Config", (string)request.Form.Config.ToString() }
                            };

            return await _agentHttpClient.Post<ServiceResult<object>>("config", address, port, arguments);
        }
    }
}