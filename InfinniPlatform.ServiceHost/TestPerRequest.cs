using System;
using System.Threading.Tasks;
using InfinniPlatform.Http;

namespace InfinniPlatform.ServiceHost
{
    public interface IInterface
    {
        Guid M();
    }

    internal class MyClass : IInterface
    {
        private readonly Guid _newGuid;

        public MyClass()
        {
            _newGuid = Guid.NewGuid();
            Console.WriteLine(Guid.NewGuid());
        }

        public Guid M()
        {
            return _newGuid;
        }
    }

    internal class Service1 : IHttpService
    {
        private readonly IInterface _a1;
        private readonly IInterface _a2;

        public Service1(IInterface a1, IInterface a2)
        {
            _a1 = a1;
            _a2 = a2;
        }

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/get"] = Func;
        }

        private Task<object> Func(IHttpRequest httpRequest)
        {
            var result = new {i1 = _a1.M(), i2 = _a2.M()};

            return Task.FromResult<object>(result);
        }
    }
}