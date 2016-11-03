using System;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.InfinniNode.Tasks
{
    public class InitAppTask : IAppTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public InitAppTask(ProcessHelper processHelper)
        {
            _processHelper = processHelper;
        }

        private readonly ProcessHelper _processHelper;

        public string CommandName => "init";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public async Task<object> Run(IHttpRequest request)
        {
            var command = CommandName.AppendArg("i", (string)request.Form.AppName)
                                     .AppendArg("v", (string)request.Form.Version)
                                     .AppendArg("n", (string)request.Form.Instance)
                                     .AppendArg("t", (string)request.Form.Timeout);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout, Guid.NewGuid().ToString("D"));

            return new TaskStatus(processResult);
        }
    }
}