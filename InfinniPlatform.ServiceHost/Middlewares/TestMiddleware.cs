using System.Threading.Tasks;
using InfinniPlatform.ServiceHost.Services;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.ServiceHost.Middlewares
{
    public class TestMiddleware
    {
        private readonly Service _service;

        public TestMiddleware(RequestDelegate next, Service service)
        {
            _service = service;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var assembly = _service.Get();
            await httpContext.Response.WriteAsync(assembly);
        }
    }
}