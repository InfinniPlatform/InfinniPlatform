using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.UserAuthHandlers
{
    /// <summary>
    ///   Регистрация обработчика для указанного роутинга запроса
    /// </summary>
    public sealed class AddUserHandlerRegistration : HandlerRegistration
    {
        public AddUserHandlerRegistration()
            : base(new RouteFormatterAuthUser(), new RequestPathConstructor(), Priority.Higher, "PUT")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context,PathConstructor.GetUserPath()).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            dynamic body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            if (body.UserName != null && body.Password != null)
            {
                return new ValueRequestHandlerResult(new UsersApi(routeDictionary["version"]).AddUser(body.UserName.ToString(), body.Password.ToString()));
            }

            return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
        }


    }


}
