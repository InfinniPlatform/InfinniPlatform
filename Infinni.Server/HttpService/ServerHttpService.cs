using System.Net.Http;
using System.Threading.Tasks;

using Infinni.Server.Properties;
using Infinni.Server.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;

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

            builder.OnError = (request, e) =>
                              {
                                  var exception = e as HttpServiceException;

                                  if (exception != null)
                                  {
                                      var errorHttpResponse = exception.ErrorHttpResponse;
                                      return Task.FromResult<object>(errorHttpResponse);
                                  }

                                  return CreateErrorResponse(Resources.UnexpectedServerError);
                              };

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

        private static Task<object> CreateErrorResponse(string errorMessage)
        {
            return Task.FromResult<object>(new JsonHttpResponse(new ServiceResult<DynamicWrapper>
                                                                {
                                                                    Success = false,
                                                                    Error = errorMessage
                                                                })
                                               { StatusCode = 500 });
        }
    }
}