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

namespace InfinniPlatform.WebApi.Middleware.VersionHandlers
{
    public sealed class GetIrrelevantVersionHandlerRegistration : HandlerRegistration
    {
        public GetIrrelevantVersionHandlerRegistration()
            : base(new RouteFormatterAuthUser(), new RequestPathConstructor(), Priority.Higher, "GET")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/_documentType_/_userName_")).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {          
            dynamic body = new DynamicWrapper();
            body.UserName = RouteFormatter.GetRouteDictionary(context)["userName"];

            return
                new ValueRequestHandlerResult(RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata",
                                                                                      "getirrelevantversions", null, body).ToDynamicList());
        }
    }
}
