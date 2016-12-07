using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;

namespace Infinni.Server.HttpService
{
    public class ServerHttpGlobalHandler : IHttpGlobalHandler
    {
        public ServerHttpGlobalHandler()
        {
            OnError = onError;
        }

        private Task<object> onError(IHttpRequest request, Exception e)
        {
            var error = new ServiceResult<DynamicWrapper>
            {
                Success = false,
                Error = "Понятное сообщение!"
            };

            var errorHttpResponse = new JsonHttpResponse(error) { StatusCode = 500 };

            return Task.FromResult<object>(errorHttpResponse);
        }

        public Func<IHttpRequest, Task<object>> OnBefore { get; set; }

        public Func<IHttpRequest, object, Task<object>> OnAfter { get; set; }

        public Func<IHttpRequest, Exception, Task<object>> OnError { get; set; }

        public Func<object, IHttpResponse> ResultConverter { get; set; }
    }
}