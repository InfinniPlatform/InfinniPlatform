using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.Solution
{
    public abstract class SolutionHandlerRegistration : HandlerRegistration
    {
        protected SolutionHandlerRegistration(string method)
            : base(new RouteFormatterMetadata(), new RequestPathConstructor(), Priority.Concrete, method)
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            if (Method == "POST" || Method == "PUT")
            {
                return new PathStringProvider
                    {
                        PathString = new PathString(PathConstructor.GetBaseApplicationPath() + "/solution"),
                        Priority = Priority
                    };
            }
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/solution/_versionMetadata_/_instanceId_")).Create(Priority);
        }
    }
}
