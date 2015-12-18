using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class CustomServiceRegistrationHandler : HandlerRegistration
    {
        private readonly RestQueryApi _restQueryApi;

        public CustomServiceRegistrationHandler(RestQueryApi restQueryApi) : base(new RouteFormatterCustomService(), new RequestPathConstructor(), Priority.Standard, "POST")
        {
            _restQueryApi = restQueryApi;
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/_documentType_/_service_")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            dynamic body = null;

            try
            {
                body = JObject.Parse(RoutingOwinMiddleware.ReadRequestBody(context).ToString());
            }
            catch (Exception e)
            {
                body = null;
            }

            dynamic result = _restQueryApi.QueryPostJsonRaw(routeDictionary["application"], routeDictionary["documentType"], routeDictionary["service"], null, body).ToDynamic();

            return new ValueRequestHandlerResult(result);
        }
    }
}