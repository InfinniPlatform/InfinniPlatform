using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class SetDocumentsHandlerRegistration : HandlerRegistration
    {
        public SetDocumentsHandlerRegistration() : base(new RouteFormatterStandard(), new RequestPathConstructor(), Owin.Middleware.Priority.Standard, "PUT")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/_documentType_")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic body = JArray.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            return new ValueRequestHandlerResult(new DocumentApi().SetDocuments(routeDictionary["application"], routeDictionary["documentType"], body));
        }
    }
}
