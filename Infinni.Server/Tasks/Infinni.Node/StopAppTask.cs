using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.PushNotification;
using Infinni.Server.Agent;
using Infinni.Server.Tasks.Agents;

namespace Infinni.Server.Tasks.Infinni.Node
{
    public class StopAppTask : IServerTask
    {
        private const string NotifyMessageType = "WorkLog";

        public StopAppTask(IAgentHttpClient agentHttpClient,
                           IPushNotificationService notifyService)
        {
            _agentHttpClient = agentHttpClient;
            _notifyService = notifyService;
        }

        private readonly IAgentHttpClient _agentHttpClient;
        private readonly IPushNotificationService _notifyService;

        public string CommandName => "stop";

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

            await _notifyService.NotifyAll(NotifyMessageType, $"Stopping {args["AppName"]}...");

            var serviceResult = await _agentHttpClient.Post<ServiceResult<AgentTaskStatus>>(CommandName, address, port, args);

            await _notifyService.NotifyAll(NotifyMessageType, $"Stopping {args["AppName"]} comleted!");

            return serviceResult;
        }
    }
}