using Autofac;

using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Core.IoC.Http
{
    internal sealed class AutofacHttpMiddleware //: HttpMiddleware
    {
        public AutofacHttpMiddleware(IContainer container)// : base(HttpMiddlewareType.GlobalHandling)
        {
            _container = container;
        }


        private readonly IContainer _container;


//        public override void Configure(IApplicationBuilder appBuilder)
//        {
//            //TODO Find way to extend OWIN pipelines in ASP.NET Core.
//            //appBuilder.UseOwin(typeof(AutofacRequestLifetimeScopeOwinMiddleware), _container);
//        }
    }
}