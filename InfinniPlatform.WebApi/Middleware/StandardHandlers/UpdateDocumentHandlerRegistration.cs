using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class UpdateDocumentHandlerRegistration : HandlerRegistration
    {
        public UpdateDocumentHandlerRegistration(DocumentApi documentApi) : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Higher, "POST")
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
            dynamic body = RoutingOwinMiddleware.ReadRequestBody(context);

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            body.Id = routeDictionary["instanceId"];

            return new ValueRequestHandlerResult(_documentApi.UpdateDocument(routeDictionary["application"], routeDictionary["documentType"], body));
        }
    }
}