using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class GetDocumentByIdHandlerRegistration : HandlerRegistration
    {
        public GetDocumentByIdHandlerRegistration(DocumentApi documentApi) : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Standard, "GET")
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

            var result = _documentApi.GetDocument(routeDictionary["application"], routeDictionary["documentType"],
                cr => cr.AddCriteria(f => f.IsEquals(routeDictionary["instanceId"]).Property("Id")), 0, 1).FirstOrDefault();

            return new ValueRequestHandlerResult(result);
        }
    }
}