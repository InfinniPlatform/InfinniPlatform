using System.Net.Http;

using InfinniPlatform.Sdk.Http.Services;
using Infinni.Server.Tasks;

namespace Infinni.Server.HttpService
{
    /// <summary>
    /// Сервис взаимодействия с приложением Infinni.Agent.
    /// </summary>
    public class ServerHttpService : IHttpService
    {
        public ServerHttpService(IServerTask[] serverTasks)
        {
            _serverTasks = serverTasks;
        }

        private readonly IServerTask[] _serverTasks;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "server";

            foreach (var serverTask in _serverTasks)
            {
                if (serverTask.HttpMethod == HttpMethod.Get)
                {
                    builder.Get[serverTask.CommandName] = request => serverTask.Run(request);
                }

                if (serverTask.HttpMethod == HttpMethod.Post)
                {
                    builder.Post[serverTask.CommandName] = request => serverTask.Run(request);
                }
            }
        }
    }
}