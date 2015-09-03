using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Converters;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class GetDocumentHandlerRegistration : HandlerRegistration
    {
        public GetDocumentHandlerRegistration() : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Standard, "GET")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/_documentType_")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            if (context.Request.QueryString.HasValue)
            {
                nameValueCollection = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(context.Request.QueryString.Value));
            }

            var filter = nameValueCollection.Get("filter");
            IEnumerable<dynamic> criteriaList = new List<dynamic>();
            if (filter != null)
            {
                criteriaList = new FilterConverter().ConvertFilter(filter);
            }

            var sorting = nameValueCollection.Get("sorting");
            IEnumerable<dynamic> sortingList = new List<dynamic>();
            if (sorting != null)
            {
                sortingList = new SortingConverter().Convert(sorting);
            }


            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            IEnumerable<dynamic> result = new DocumentApi().GetDocument(routeDictionary["application"], routeDictionary["documentType"], criteriaList,
                Convert.ToInt32(nameValueCollection["pagenumber"]), Convert.ToInt32(nameValueCollection["pageSize"]), null, sortingList);

            return new ValueRequestHandlerResult(result);
        }
    }
}
