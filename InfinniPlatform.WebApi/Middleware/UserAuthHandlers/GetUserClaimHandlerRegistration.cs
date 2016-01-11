using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.UserAuthHandlers
{
    public sealed class GetUserClaimHandlerRegistration : HandlerRegistration
    {
        public GetUserClaimHandlerRegistration(ISessionManager sessionManager) : base(new RouteFormatterAuthUser(), new RequestPathConstructor(), Priority.Higher, "GET")
        {
            _sessionManager = sessionManager;
        }

        private readonly ISessionManager _sessionManager;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetSpecifiedUserClaimPath()).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var sessionDataKey = routeDictionary["claimType"];
            var sessionDataValue = _sessionManager.GetSessionData(sessionDataKey);

            var result = new DynamicWrapper { ["IsValid"] = true, ["ClaimValue"] = sessionDataValue };

            return new ValueRequestHandlerResult(result);
        }
    }
}