using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.UserAuthHandlers
{
    public sealed class AddUserClaimHandlerRegistration : HandlerRegistration
    {
        public AddUserClaimHandlerRegistration(AuthApi authApi) : base(new RouteFormatterAuthUser(), new RequestPathConstructor(), Priority.Higher, "PUT")
        {
            _authApi = authApi;
        }

        private readonly AuthApi _authApi;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetSpecifiedUserClaimPath()).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            return new ValueRequestHandlerResult(_authApi.SetSessionData(routeDictionary["userName"], routeDictionary["claimType"], body.ClaimValue != null ? body.ClaimValue.ToString() : null));
        }
    }
}