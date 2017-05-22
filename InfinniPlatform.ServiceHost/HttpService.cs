using System.Threading.Tasks;
using InfinniPlatform.Http;

namespace InfinniPlatform.ServiceHost
{
    public class HttpService : IHttpService
    {
        private readonly IInterface _i1;
        private readonly IInterface _i2;

        public HttpService(IInterface i1, IInterface i2)
        {
            _i1 = i1;
            _i2 = i2;
        }

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/get"] = Get;
        }

        private Task<object> Get(IHttpRequest httpRequest)
        {
            var foo = new {i1 = _i1.M(), i2 = _i2.M()};

            return Task.FromResult<object>(foo);
        }
    }
}