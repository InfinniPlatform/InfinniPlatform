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
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.WebApi.Middleware.SessionHandlers
{
    public sealed class FileDetachHandlerRegistration : HandlerRegistration
    {
        public FileDetachHandlerRegistration()
            : base(new RouteFormatterSession(), new RequestPathConstructor(), Priority.Standard, "DELETE")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetVersionPath()).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            if (body.InstanceId != null && body.FieldName != null && body.SessionId != null)
            {
                return new ValueRequestHandlerResult(new SessionApi(routeDictionary["version"]).DetachFile(body));
            }
            return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
        }
    }
}
