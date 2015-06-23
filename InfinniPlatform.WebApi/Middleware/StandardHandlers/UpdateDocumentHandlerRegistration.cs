using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class UpdateDocumentHandlerRegistration : HandlerRegistration
    {
        public UpdateDocumentHandlerRegistration() : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Higher, "POST")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/_documentType_/_instanceId_")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString()).ToDynamic();

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            body.Id = routeDictionary["instanceId"];

            return new ValueRequestHandlerResult(new DocumentApi(routeDictionary["version"]).UpdateDocument(routeDictionary["application"], routeDictionary["documentType"], body));
        }
    }
}
