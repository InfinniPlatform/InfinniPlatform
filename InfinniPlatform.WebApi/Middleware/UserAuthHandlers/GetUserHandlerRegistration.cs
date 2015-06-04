using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.UserAuthHandlers
{
    public sealed class GetUserHandlerRegistration : HandlerRegistration
    {
        public GetUserHandlerRegistration() : base(new RouteFormatterAuthUser(), new RequestPathConstructor(), Priority.Higher, "GET")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetSpecifiedUserPath()).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            return new ValueRequestHandlerResult(new UsersApi().GetUser(RouteFormatter.GetRouteDictionary(context)["userName"]));
        }
    }
}
