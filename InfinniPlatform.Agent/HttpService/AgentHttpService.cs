using System.Net.Http;

using InfinniPlatform.Agent.Tasks;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.HttpService
{
    /// <summary>
    /// Сервис взаимодействия с приложением Infinni.Node.
    /// </summary>
    public class AgentHttpService : IHttpService
    {
        public AgentHttpService(IAgentTask[] appTasks)
        {
            _appTasks = appTasks;
        }

        private readonly IAgentTask[] _appTasks;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "agent";

            foreach (var appTask in _appTasks)
            {
                if (appTask.HttpMethod == HttpMethod.Get)
                {
                    builder.Get[appTask.CommandName] = request => appTask.Run(request);
                }

                if (appTask.HttpMethod == HttpMethod.Post)
                {
                    builder.Post[appTask.CommandName] = request => appTask.Run(request);
                }
            }
        }
    }
}