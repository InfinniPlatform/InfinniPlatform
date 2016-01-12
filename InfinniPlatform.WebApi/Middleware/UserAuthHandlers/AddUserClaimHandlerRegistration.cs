using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.UserAuthHandlers
{
    public sealed class AddUserClaimHandlerRegistration : HandlerRegistration
    {
        public AddUserClaimHandlerRegistration(ISessionManager sessionManager) : base(new RouteFormatterAuthUser(), new RequestPathConstructor(), Priority.Higher, "PUT")
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

            dynamic body = RoutingOwinMiddleware.ReadRequestBody(context);

            var sessionDataKey = routeDictionary["claimType"];
            var sessionDataValue = body.ClaimValue?.ToString();

            _sessionManager.SetSessionData(sessionDataKey, sessionDataValue);

            var result = new DynamicWrapper { ["IsValid"] = true };

            return new ValueRequestHandlerResult(result);
        }
    }
}