using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal static class RequestHandlerHelper
    {
        public static IHttpResponse BadRequest(string message)
        {
            // TODO: Сделать строготипизированный ответ.

            var error = new DynamicWrapper
            {
                ["IsValid"] = false,
                ["ValidationMessage"] = message
            };

            return new JsonHttpResponse(error) { StatusCode = 400 };
        }
    }
}