﻿using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using Infinni.Server.Agent;

namespace Infinni.Server.Tasks.Files
{
    public class AppLogTask : IServerTask
    {
        public AppLogTask(AgentHttpClient agentHttpClient)
        {
            _agentHttpClient = agentHttpClient;
        }

        private readonly AgentHttpClient _agentHttpClient;

        public string CommandName => "appLog";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public async Task<object> Run(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "FullName", (string)request.Query.FullName }
                            };

            var stream = await _agentHttpClient.GetStream(CommandName, address, port, arguments);

            return new StreamHttpResponse(() => stream, HttpConstants.TextPlainContentType);
        }
    }
}