using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Infinni.Server.Tasks.Infinni.Node;

using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Logging;

namespace Infinni.Server.Tasks.Heartbeat
{
    public class LogHeartBeatTask : IServerTask
    {
        public LogHeartBeatTask(ILog log)
        {
            _log = log;
        }

        private readonly ILog _log;

        public string CommandName => "heartbeat";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public Task<object> Run(IHttpRequest request)
        {
            string s = request.Form.Message;

            _log.Info(s, () => new Dictionary<string, object>
                               {
                                   { "Name", HttpServiceHelper.ParseString(request.Form.Name) },
                                   { "InstanceId", HttpServiceHelper.ParseString(request.Form.InstanceId) },
                                   { "Time", DateTime.Now.ToString("O") }
                               });

            var serviceResult = new ServiceResult<object>
                                {
                                    Success = true
                                };

            return Task.FromResult<object>(serviceResult);
        }
    }
}