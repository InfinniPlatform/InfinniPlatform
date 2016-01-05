using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class CustomServiceRegistrationHandler : HandlerRegistration
    {
        public CustomServiceRegistrationHandler(RestQueryApi restQueryApi) : base(new RouteFormatterCustomService(), new RequestPathConstructor(), Priority.Standard, "POST")
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/_documentType_/_service_")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            dynamic body = RoutingOwinMiddleware.ReadRequestBody(context);

            dynamic result = _restQueryApi.QueryPostJsonRaw(routeDictionary["application"], routeDictionary["documentType"], routeDictionary["service"], null, body).ToDynamic();

            return new ValueRequestHandlerResult(result);
        }
    }
}