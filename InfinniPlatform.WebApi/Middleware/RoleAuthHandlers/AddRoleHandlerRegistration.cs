using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.RoleAuthHandlers
{
    public sealed class AddRoleHandlerRegistration : HandlerRegistration
    {
        public AddRoleHandlerRegistration() : base(new RouteFormatterAuthRole(), new RequestPathConstructor(), Owin.Middleware.Priority.Higher, "PUT")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetSpecifiedRolePath()).Create(Priority.Higher);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            string roleName = routeDictionary["roleName"];

            return new ValueRequestHandlerResult(new UsersApi().AddRole(roleName, roleName, roleName));
        }
    }
}
