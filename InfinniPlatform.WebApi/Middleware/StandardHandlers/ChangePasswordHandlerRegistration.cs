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
    public sealed class ChangePasswordHandlerRegistration : HandlerRegistration
    {
        public ChangePasswordHandlerRegistration() : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Higher, "POST")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/changepassword")).Create(Priority.Higher);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            if (body.UserName == null || body.OldPassword == null || body.NewPassword == null)
            {
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }

            try
            {
                return new ValueRequestHandlerResult(new SignInApi().ChangePassword(body.UserName.ToString(), body.OldPassword.ToString(),
                     body.NewPassword.ToString()));
            }
            catch (Exception e)
            {
                return new ErrorRequestHandlerResult(e.Message);
            }     
        }
    }
}
