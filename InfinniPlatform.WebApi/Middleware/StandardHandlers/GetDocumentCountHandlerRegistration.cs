using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.SearchOptions.Converters;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public class GetDocumentCountHandlerRegistration : HandlerRegistration
    {
        public GetDocumentCountHandlerRegistration(DocumentApi documentApi) : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Standard, "GET")
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/_documentType_/$count")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var nameValueCollection = new NameValueCollection();
            if (context.Request.QueryString.HasValue)
            {
                nameValueCollection = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(context.Request.QueryString.Value));
            }

            var filter = nameValueCollection.Get("$filter");
            IEnumerable<dynamic> criteriaList = new List<dynamic>();
            if (filter != null)
            {
                criteriaList = new FilterConverter().ConvertFilter(filter);
            }

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var result = _documentApi.GetNumberOfDocuments(routeDictionary["application"], routeDictionary["documentType"], criteriaList);

            return new ValueRequestHandlerResult(result);
        }
    }
}