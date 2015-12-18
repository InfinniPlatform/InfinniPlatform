using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.UserAuthHandlers
{
    public sealed class GetUserClaimHandlerRegistration : HandlerRegistration
    {
        public GetUserClaimHandlerRegistration(AuthApi authApi) : base(new RouteFormatterAuthUser(), new RequestPathConstructor(), Priority.Higher, "GET")
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

            return new ValueRequestHandlerResult(_authApi.GetSessionData(routeDictionary["userName"], routeDictionary["claimType"]));
        }
    }
}