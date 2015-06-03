using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class SignOutRegistrationHandler : HandlerRegistration
    {
        public SignOutRegistrationHandler() : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Higher,"POST")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/signout")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic result = null;
            try
            {
                result = new SignInApi().SignOutInternal();
            }
            catch (Exception e)
            {
                return new ErrorRequestHandlerResult(e.Message);
            }

            return new ValueRequestHandlerResult(result);
        }
    }
}
