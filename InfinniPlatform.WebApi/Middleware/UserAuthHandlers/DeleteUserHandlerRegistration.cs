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
    public sealed class DeleteUserHandlerRegistration : HandlerRegistration
    {
        public DeleteUserHandlerRegistration() : base(new RouteFormatterAuthUser(), new RequestPathConstructor(), Priority.Higher, "DELETE")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetSpecifiedUserPath())
                .Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            return new ValueRequestHandlerResult(new UsersApi(routeDictionary["version"]).RemoveUser(RouteFormatter.GetRouteDictionary(context)["userName"]));
        }
    }
}
