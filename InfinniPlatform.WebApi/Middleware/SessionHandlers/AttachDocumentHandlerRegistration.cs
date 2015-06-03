using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.SessionHandlers
{
    public sealed class AttachDocumentHandlerRegistration : HandlerRegistration
    {
        public AttachDocumentHandlerRegistration() : base(new RouteFormatterSession(), new RequestPathConstructor(), Priority.Standard, "PUT")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/_sessionId_")).Create(Priority.Standard);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            return new ValueRequestHandlerResult(new SessionApi().Attach(routeDictionary["version"], routeDictionary["sessionId"], body));
        }
    }
}
