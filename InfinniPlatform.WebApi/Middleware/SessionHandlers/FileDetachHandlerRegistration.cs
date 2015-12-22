using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.SessionHandlers
{
    public sealed class FileDetachHandlerRegistration : HandlerRegistration
    {
        public FileDetachHandlerRegistration(SessionApi sessionApi)
            : base(new RouteFormatterSession(), new RequestPathConstructor(), Priority.Standard, "DELETE")
        {
            _sessionApi = sessionApi;
        }

        private readonly SessionApi _sessionApi;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetVersionPath()).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic body = RoutingOwinMiddleware.ReadRequestBody(context);

            if (body.InstanceId != null && body.FieldName != null && body.SessionId != null)
            {
                return new ValueRequestHandlerResult(_sessionApi.DetachFile(body));
            }

            return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
        }
    }
}