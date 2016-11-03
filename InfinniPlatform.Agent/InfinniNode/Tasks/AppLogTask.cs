﻿using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.InfinniNode.Providers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.InfinniNode.Tasks
{
    public class AppLogTask : IAppTask
    {
        public AppLogTask(ILogFilePovider logFilePovider)
        {
            _logFilePovider = logFilePovider;
        }

        private readonly ILogFilePovider _logFilePovider;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "appLog";

        public Task<object> Run(IHttpRequest request)
        {
            string appFullName = request.Query.AppFullName;

            var streamHttpResponse = new StreamHttpResponse(_logFilePovider.GetAppLog(appFullName), "application/text");

            return Task.FromResult<object>(streamHttpResponse);
        }
    }
}