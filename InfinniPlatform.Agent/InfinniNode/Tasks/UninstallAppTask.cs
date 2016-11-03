using System;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.InfinniNode.Tasks
{
    public class UninstallAppTask : IAppTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public UninstallAppTask(ProcessHelper processHelper)
        {
            _processHelper = processHelper;
        }

        private readonly ProcessHelper _processHelper;

        public HttpMethod HttpMethod => HttpMethod.Post;

        public string CommandName => "uninstall";

        public async Task<object> Run(IHttpRequest request)
        {
            var command = CommandName.AppendArg("i", (string)request.Form.AppName)
                                     .AppendArg("v", (string)request.Form.Version)
                                     .AppendArg("n", (string)request.Form.Instance);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout, Guid.NewGuid().ToString("D"));

            var taskStatus = new TaskStatus(processResult);

            return taskStatus;
        }
    }
}