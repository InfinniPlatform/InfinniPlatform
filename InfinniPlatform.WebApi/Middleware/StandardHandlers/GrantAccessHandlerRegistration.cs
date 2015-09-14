using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class GrantAccessHandlerRegistration : HandlerRegistration
    {
        public GrantAccessHandlerRegistration() : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Higher, "POST")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return
                RouteFormatter.FormatRoutePath(context,new PathString(PathConstructor.GetUserPath() + "/GrantAccess"))
                    .Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            if (body.Application != null && body.UserName != null && body.Application.ToString() != string.Empty && body.UserName.ToString() != string.Empty)
            {
                return new ValueRequestHandlerResult(new AuthApi().GrantAccess(
                    body.UserName.ToString(),
                    body.Application.ToString(),
                    body.DocumentType != null ? body.DocumentType.ToString() : null,
                    body.Service != null ? body.Service.ToString() : null,
                    body.InstanceId != null ? body.InstanceId.ToString() : null));
            }

            return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
        }
    }
}
