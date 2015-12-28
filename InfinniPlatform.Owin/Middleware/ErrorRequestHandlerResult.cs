using System.Net;
using System.Threading.Tasks;

using InfinniPlatform.Owin.Formatting;
using InfinniPlatform.Sdk.Dynamic;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    /// Результат неуспешной обработки HTTP-запроса в виде сообщения об ошибке.
    /// </summary>
    public sealed class ErrorRequestHandlerResult : IRequestHandlerResult
    {
        public ErrorRequestHandlerResult(object error)
        {
            _error = error;
        }


        private readonly object _error;


        public bool IsSuccess => false;

        public Task GetResult(IOwinContext context)
        {
            // TODO: Сейчас все обработчики ожидают именно такой формат ответа

            var error = new DynamicWrapper
            {
                ["IsValid"] = false,
                ["IsInternalServerError"] = true,
                ["ValidationMessage"] = _error
            };

            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return JsonBodyFormatter.Instance.WriteBody(response, error);
        }
    }
}