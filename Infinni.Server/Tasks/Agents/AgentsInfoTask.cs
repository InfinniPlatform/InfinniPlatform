using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;
using Infinni.Server.Settings;

namespace Infinni.Server.Tasks.Agents
{
    /// <summary>
    /// Возвращает информацию об известных агентах.
    /// </summary>
    public class AgentsInfoTask : IServerTask
    {
        public AgentsInfoTask(AgentsInfoProvider agentsInfoProvider)
        {
            _agentsInfoProvider = agentsInfoProvider;
        }

        private readonly AgentsInfoProvider _agentsInfoProvider;


        public string CommandName => "agents";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public Task<object> Run(IHttpRequest request)
        {
            var agentsInfoList = _agentsInfoProvider.GetAgentsInfoList();

            var serviceResult = new ServiceResult<List<AgentInfo>> { Success = true, Result = agentsInfoList };

            return Task.FromResult<object>(serviceResult);
        }
    }
}