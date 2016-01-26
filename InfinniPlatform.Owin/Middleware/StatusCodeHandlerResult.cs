using System.Threading.Tasks;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    public class StatusCodeHandlerResult : IRequestHandlerResult
    {
        public StatusCodeHandlerResult(int code)
        {
            _code = code;
        }

        private readonly int _code;

        public Task GetResult(IOwinContext context)
        {
            context.Response.StatusCode = _code;
            return Task.FromResult(_code);
        }
    }
}