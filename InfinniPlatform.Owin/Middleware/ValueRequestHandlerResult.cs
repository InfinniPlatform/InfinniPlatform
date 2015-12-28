using System.Net;
using System.Threading.Tasks;

using InfinniPlatform.Owin.Formatting;
using InfinniPlatform.Sdk.Dynamic;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    /// Результат успешной обработки HTTP-запроса в виде значения.
    /// </summary>
    public sealed class ValueRequestHandlerResult : IRequestHandlerResult
    {
        public ValueRequestHandlerResult(object value)
        {
            _value = value;
        }


        private readonly object _value;

        public bool IsSuccess => IsSuccessValue(_value);

        public Task GetResult(IOwinContext context)
        {
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.OK;
            return JsonBodyFormatter.Instance.WriteBody(response, _value);
        }

        private static bool IsSuccessValue(object value)
        {
            // TODO: На данном этапе рефакторинга пока удалось сделать только так

            try
            {
                return !Equals(value.GetProperty("IsValid"), false);
            }
            catch
            {
                return true;
            }
        }
    }
}