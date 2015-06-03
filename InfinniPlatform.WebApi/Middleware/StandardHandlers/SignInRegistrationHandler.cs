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
    public sealed class SignInRegistrationHandler : HandlerRegistration
    {
        public SignInRegistrationHandler() : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Higher, "POST")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context,new PathString(PathConstructor.GetVersionPath() + "/signin")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            if (body.UserName == null || body.Password == null || body.Remember == null)
            {
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }

            return new ValueRequestHandlerResult(new SignInApi().SignInInternal(body.UserName.ToString(), body.Password.ToString(), Convert.ToBoolean(body.Remember)));
        }
    }
}
