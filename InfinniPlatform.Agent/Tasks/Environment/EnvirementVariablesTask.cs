using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Providers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.Environment
{
    public class EnvirementVariablesTask : IAppTask
    {
        public EnvirementVariablesTask(IEnvironmentVariableProvider variableProvider)
        {
            _variableProvider = variableProvider;
        }

        private readonly IEnvironmentVariableProvider _variableProvider;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "variables";

        public Task<object> Run(IHttpRequest request)
        {
            var variables = _variableProvider.GetAll();

            return Task.FromResult<object>(new ServiceResult<IDictionary>
                                           {
                                               Success = true,
                                               Result = variables
                                           });
        }
    }
}