using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.PushNotification;
using InfinniPlatform.Server.Agent;
using InfinniPlatform.Server.Tasks.Agents;

namespace InfinniPlatform.Server.Tasks.Infinni.Node
{
    public class InstallAppTask : IServerTask
    {
        private const string InstallMessageType = "Install";

        public InstallAppTask(IAgentHttpClient agentHttpClient,
                              IPushNotificationService notifyService)
        {
            _notifyService = notifyService;
            _agentHttpClient = agentHttpClient;
        }

        private readonly IAgentHttpClient _agentHttpClient;
        private readonly IPushNotificationService _notifyService;

        public string CommandName => "install";

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
                           { "Source", HttpServiceHelper.ParseString(request.Form.Source) },
                           { "AllowPrerelease", (bool?)request.Form.AllowPrerelease }
                       };

            await _notifyService.NotifyAll(InstallMessageType, new DynamicWrapper
                                                               {
                                                                   { "AppName", args["AppName"] },
                                                                   { "Version", args["Version"] },
                                                                   { "Instance", args["Instance"] },
                                                                   { "State", "Installing" }
                                                               });

            var serviceResult = await _agentHttpClient.Post<ServiceResult<AgentTaskStatus>>("install", address, port, args);


            return serviceResult;
        }
    }
}