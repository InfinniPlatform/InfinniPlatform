using System;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.InfinniNode.Tasks
{
    public class AppsInfoTask : IAppTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public AppsInfoTask(ProcessHelper processHelper)
        {
            _processHelper = processHelper;
        }

        private readonly ProcessHelper _processHelper;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "status";

        public async Task<object> Run(IHttpRequest request)
        {
            var processResult = await _processHelper.ExecuteCommand(CommandName, ProcessTimeout, Guid.NewGuid().ToString("D"));

            return new TaskStatus(processResult);
        }
    }
}