using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.PushNotification;
using InfinniPlatform.Server.Agent;
using InfinniPlatform.Server.Tasks.Agents;

namespace InfinniPlatform.Server.Tasks.Infinni.Node
{
    public class RestartAppTask : IServerTask
    {
        private const string NotifyMessageType = "WorkLog";

        public RestartAppTask(IAgentHttpClient agentHttpClient,
                              IPushNotificationService notifyService)
        {
            _agentHttpClient = agentHttpClient;
            _notifyService = notifyService;
        }

        private readonly IAgentHttpClient _agentHttpClient;
        private readonly IPushNotificationService _notifyService;

        public string CommandName => "restart";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public async Task<object> Run(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var args = new DynamicWrapper
                       {
                           { "AppName", HttpServiceHelper.ParseString(request.Form.AppName) },
                           { "Version", HttpServiceHelper.ParseString(request.Form.Version) },
                           { "Instance", HttpServiceHelper.ParseString(request.Form.Instance) },
                           { "Timeout", HttpServiceHelper.ParseInt(request.Form.Timeout) }
                       };

            await _notifyService.NotifyAll(NotifyMessageType, $"Restarting {args["AppName"]}...");

            var serviceResult = await _agentHttpClient.Post<ServiceResult<AgentTaskStatus>>(CommandName, address, port, args);

            await _notifyService.NotifyAll(NotifyMessageType, $"Restarting {args["AppName"]}...");

            return serviceResult;
        }
    }
}