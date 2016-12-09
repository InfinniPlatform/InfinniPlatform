using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Infinni.Server.Properties;
using Infinni.Server.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Logging;

using Newtonsoft.Json;

namespace Infinni.Server.HttpService
{
    /// <summary>
    /// Сервис взаимодействия с приложением Infinni.Agent.
    /// </summary>
    public class ServerHttpService : IHttpService
    {
        public ServerHttpService(IServerTask[] serverTasks, ILog log)
        {
            _exceptionsWrappers = new Dictionary<Type, Func<string, HttpResponse>>
                                  {
                                      { typeof(UriFormatException), message => CreateErrorResponse(Resources.CheckUriFormat) },
                                      { typeof(HttpRequestException), message => CreateErrorResponse(Resources.UnableConnectAgent) },
                                      { typeof(JsonReaderException), message => CreateErrorResponse(Resources.UnableReadAgentResponse) },
                                      { typeof(HttpServiceException), CreateErrorResponse }
                                  };

            _serverTasks = serverTasks;
            _log = log;
        }

        private readonly Dictionary<Type, Func<string, HttpResponse>> _exceptionsWrappers;

        private readonly ILog _log;

        private readonly IServerTask[] _serverTasks;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "server";

            builder.OnError = (request, e) =>
                              {
                                  _log.Error(e);

                                  var exceptionType = e.GetType();

                                  var httpResponse = _exceptionsWrappers.ContainsKey(exceptionType)
                                      ? _exceptionsWrappers[exceptionType].Invoke(e.Message)
                                      : CreateErrorResponse(Resources.UnexpectedServerError);

                                  return Task.FromResult<object>(httpResponse);
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

        private static JsonHttpResponse CreateErrorResponse(string errorMessage)
        {
            return new JsonHttpResponse(new ServiceResult<DynamicWrapper>
                                        {
                                            Success = false,
                                            Error = errorMessage
                                        })
                       { StatusCode = 500 };
        }
    }
}