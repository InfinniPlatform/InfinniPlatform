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

namespace InfinniPlatform.WebApi.Middleware.UserAuthHandlers
{
    public sealed class AddUserClaimHandlerRegistration : HandlerRegistration
    {
        public AddUserClaimHandlerRegistration() : base(new RouteFormatterAuthUser(), new RequestPathConstructor(), Priority.Higher, "PUT")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context,PathConstructor.GetSpecifiedUserClaimPath())
                .Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            return new ValueRequestHandlerResult(new AuthApi().SetSessionData(routeDictionary["userName"], routeDictionary["claimType"], body.ClaimValue != null ? body.ClaimValue.ToString() : null));

        }
    }
}
