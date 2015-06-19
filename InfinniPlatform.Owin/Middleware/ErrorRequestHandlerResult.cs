using System.Net;
using System.Threading.Tasks;
using InfinniPlatform.Owin.Formatting;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///     Результат неуспешной обработки HTTP-запроса в виде сообщения об ошибке.
    /// </summary>
    public sealed class ErrorRequestHandlerResult : IRequestHandlerResult
    {
        private readonly object _error;

        public ErrorRequestHandlerResult(object error)
        {
            _error = error;
        }

        public Task GetResult(IOwinContext context)
        {
            var response = context.Response;
            response.StatusCode = (int) HttpStatusCode.BadRequest;
            return JsonBodyFormatter.Instance.WriteBody(response, new {Error = _error});
        }
    }
}