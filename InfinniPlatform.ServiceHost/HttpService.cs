using System;
using System.Threading.Tasks;
using InfinniPlatform.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.ServiceHost
{
    public class HttpService : IHttpService
    {
        private readonly IInterface _i1;
        private readonly IInterface _i2;
        private readonly IHttpContextAccessor _httpContext;

        public HttpService(IInterface i1, IInterface i2, IHttpContextAccessor httpContext)
        {
            _i1 = i1;
            _i2 = i2;
            _httpContext = httpContext;
        }

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/get"] = Get;
        }

        private Task<object> Get(IHttpRequest httpRequest)
        {

            var httpContext = _httpContext.HttpContext;

            IServiceProvider services = httpContext.RequestServices;

            var i1 = services.GetRequiredService<IInterface>();
            var i2 = services.GetRequiredService<IInterface>();

            //_httpContext.HttpContext.RequestServices.

            var foo = new {i1 = i1.M(), i2 = i2.M()};

            return Task.FromResult<object>(foo);
        }
    }
}