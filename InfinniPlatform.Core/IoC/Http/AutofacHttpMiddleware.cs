using Autofac;

namespace InfinniPlatform.IoC.Http
{
    internal sealed class AutofacHttpMiddleware //: HttpMiddleware
    {
        private readonly IContainer _container;

        public AutofacHttpMiddleware(IContainer container) // : base(HttpMiddlewareType.GlobalHandling)
        {
            _container = container;
        }


//        public override void Configure(IApplicationBuilder appBuilder)
//        {
//            //TODO Вероятно данный этап уже не нужен. См. http://docs.autofac.org/en/latest/integration/aspnetcore.html?highlight=core#differences-from-asp-net-classic
//            appBuilder.UseOwin(typeof(AutofacRequestLifetimeScopeOwinMiddleware), _container);
//        }
    }
}