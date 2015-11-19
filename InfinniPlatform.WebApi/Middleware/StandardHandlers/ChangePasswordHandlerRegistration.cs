using System;

using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class ChangePasswordHandlerRegistration : HandlerRegistration
    {
        public ChangePasswordHandlerRegistration() : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Higher, "POST")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/changepassword")).Create(Priority.Higher);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            throw new NotSupportedException("Сервис SignInApi был удален ввиду своей неактуальности.");
        }
    }
}