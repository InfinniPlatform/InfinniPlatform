using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Server.Tasks
{
    public interface IServerTask
    {
        string CommandName { get; }

        HttpMethod HttpMethod { get; }

        Task<object> Run(IHttpRequest request);
    }
}