using System.Net;
using System.Threading.Tasks;
using InfinniPlatform.Owin.Formatting;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///     Результат успешной обработки HTTP-запроса в виде значения.
    /// </summary>
    public sealed class ValueRequestHandlerResult : IRequestHandlerResult
    {
        private readonly object _value;

        public ValueRequestHandlerResult(object value)
        {
            _value = value;
        }

        public Task GetResult(IOwinContext context)
        {
            var response = context.Response;
            response.StatusCode = (int) HttpStatusCode.OK;
            return JsonBodyFormatter.Instance.WriteBody(response, _value);
        }
    }
}