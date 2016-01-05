using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class DeleteDocumentHandlerRegistration : HandlerRegistration
    {
        public DeleteDocumentHandlerRegistration(DocumentApi documentApi) : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Standard, "DELETE")
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
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            return new ValueRequestHandlerResult(_documentApi.DeleteDocument(routeDictionary["application"], routeDictionary["documentType"], routeDictionary["instanceId"]));
        }
    }
}