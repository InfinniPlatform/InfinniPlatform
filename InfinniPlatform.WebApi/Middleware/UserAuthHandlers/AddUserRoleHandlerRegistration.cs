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

    public sealed class AddUserRoleHandlerRegistration : HandlerRegistration
    {
        public AddUserRoleHandlerRegistration() : base(new RouteFormatterAuthUser(), new RequestPathConstructor(), Priority.Higher, "PUT")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetSpecifiedUserRolePath()).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            return new ValueRequestHandlerResult(new UsersApi().AddUserRole(RouteFormatter.GetRouteDictionary(context)["userName"], RouteFormatter.GetRouteDictionary(context)["roleName"]));
        }
    }
}
