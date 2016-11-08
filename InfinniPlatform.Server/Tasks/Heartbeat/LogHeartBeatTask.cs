using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Server.Tasks.Infinni.Node;

namespace InfinniPlatform.Server.Tasks.Heartbeat
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
                                   { "InstanceId", HttpServiceHelper.ParseString(request.Form.InstanceId) }
                               });

            var serviceResult = new ServiceResult<object>
                                {
                                    Success = true
                                };

            return Task.FromResult<object>(serviceResult);
        }
    }
}