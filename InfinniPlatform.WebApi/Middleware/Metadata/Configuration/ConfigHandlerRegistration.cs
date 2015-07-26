using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Configuration
{
    public abstract class ConfigHandlerRegistration : HandlerRegistration
    {
        protected ConfigHandlerRegistration(string method)
            : base(new RouteFormatterMetadata(), new RequestPathConstructor(), Priority.Concrete, method)
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            if (Method == "POST" || Method == "PUT")
            {
                return new PathStringProvider
                    {
                        PathString = new PathString("/metadata/configuration"),
                        Priority = Priority
                    };
            }
            return RouteFormatter.FormatRoutePath(context, new PathString("/metadata/configuration/_version_/_instanceId_")).Create(Priority);
        }
    }
}
