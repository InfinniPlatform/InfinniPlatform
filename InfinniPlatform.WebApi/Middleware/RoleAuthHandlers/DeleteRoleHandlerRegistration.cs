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
    public sealed class DeleteRoleHandlerRegistration : HandlerRegistration
    {
        public DeleteRoleHandlerRegistration() : base(new RouteFormatterAuthRole(), new RequestPathConstructor(), Owin.Middleware.Priority.Higher, "DELETE")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetSpecifiedRolePath()).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            return new ValueRequestHandlerResult(new UsersApi().DeleteRole(routeDictionary["roleName"]));
        }
    }
}
