using System;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;

namespace Infinni.Server.HttpService
{
    /// <summary>
    /// Исключение при работе <see cref="ServerHttpService" />
    /// </summary>
    public class HttpServiceException : Exception
    {
        public HttpServiceException(string errorMessage)
        {
            ErrorHttpResponse = CreateErrorResponse(errorMessage);
        }

        public HttpResponse ErrorHttpResponse { get; private set; }

        private static HttpResponse CreateErrorResponse(string errorMessage)
        {
            var content = new ServiceResult<DynamicWrapper>
                          {
                              Success = false,
                              Error = errorMessage
                          };

            var response = new JsonHttpResponse(content) { StatusCode = 500 };

            return response;
        }
    }
}