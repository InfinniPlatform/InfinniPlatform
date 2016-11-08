using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks
{
    public interface IAgentTask
    {
        string CommandName { get; }

        HttpMethod HttpMethod { get; }

        Task<object> Run(IHttpRequest request);
    }
}