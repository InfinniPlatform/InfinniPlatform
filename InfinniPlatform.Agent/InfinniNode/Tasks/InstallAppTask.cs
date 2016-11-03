using System;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.InfinniNode.Tasks
{
    public class InstallAppTask : IAppTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public InstallAppTask(ProcessHelper processHelper)
        {
            _processHelper = processHelper;
        }

        private readonly ProcessHelper _processHelper;

        public string CommandName => "install";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public async Task<object> Run(IHttpRequest request)
        {
            var command = CommandName.AppendArg("i", (string)request.Form.AppName)
                                     .AppendArg("v", (string)request.Form.Version)
                                     .AppendArg("n", (string)request.Form.instance)
                                     .AppendArg("s", (string)request.Form.Source)
                                     .AppendArg("p", (bool?)request.Form.AllowPrerelease);

            var taskId = Guid.NewGuid().ToString("D");

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout, taskId);

            return new TaskStatus(processResult);
        }
    }
}