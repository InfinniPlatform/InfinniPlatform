using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class SetDocumentHandlerRegistration : HandlerRegistration
    {
        public SetDocumentHandlerRegistration(DocumentApi documentApi) : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Standard, "PUT")
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/_documentType_/_instanceId_")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            body.Id = routeDictionary["instanceId"];

            return new ValueRequestHandlerResult(_documentApi.SetDocument(routeDictionary["application"], routeDictionary["documentType"], body));
        }
    }
}