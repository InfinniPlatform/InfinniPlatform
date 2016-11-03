using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.InfinniNode.Tasks
{
    public interface IAppTask
    {
        HttpMethod HttpMethod { get; }

        string CommandName { get; }

        Task<object> Run(IHttpRequest request);
    }
}