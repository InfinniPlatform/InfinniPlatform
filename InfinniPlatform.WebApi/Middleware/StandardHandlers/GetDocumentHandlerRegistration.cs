using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

using Newtonsoft.Json;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class GetDocumentHandlerRegistration : HandlerRegistration
    {
        public GetDocumentHandlerRegistration(DocumentApi documentApi) : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Standard, "GET")
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/_documentType_")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var nameValueCollection = new NameValueCollection();
            if (context.Request.QueryString.HasValue)
            {
                nameValueCollection = HttpUtility.ParseQueryString(context.Request.QueryString.Value);
            }

            var filter = nameValueCollection.Get("filter");
            var criteriaList = JsonConvert.DeserializeObject<IEnumerable<FilterBuilder.CriteriaBuilder.CriteriaFilter>>(filter);

            var sorting = nameValueCollection.Get("sorting");
            var sortingList = JsonConvert.DeserializeObject<IEnumerable<CriteriaSorting>>(sorting);

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var result = _documentApi.GetDocument(routeDictionary["application"], routeDictionary["documentType"], criteriaList,
                Convert.ToInt32(nameValueCollection["pagenumber"]), Convert.ToInt32(nameValueCollection["pageSize"]), null, sortingList);

            return new ValueRequestHandlerResult(result);
        }
    }
}
