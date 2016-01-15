using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal static class RequestHandlerHelper
    {
        public static IHttpResponse BadRequest(string message)
        {
            // TODO: Сделать строготипизированный ответ.

            dynamic result = new DynamicWrapper();
            result.IsValid = false;
            result.IsInternalServerError = true;
            result.ValidationMessage = message;

            return new JsonHttpResponse(result) { StatusCode = 400 };
        }
    }
}