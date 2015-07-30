using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using Microsoft.Owin;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Owin.Middleware;

namespace InfinniPlatform.WebApi.Middleware.RouteFormatters
{
    public sealed class RouteFormatterStandard : IRouteFormatter
    {
        public Dictionary<string, string> GetRouteDictionary(IOwinContext context)
        {
            Guid instanceGuid = Guid.Empty;

            int versionInt = 0;
            

            var routeValues = context.Request.Path.HasValue
    ? context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

            var version = routeValues.Count() > 0 ? Int32.TryParse(routeValues[0], out versionInt) ? routeValues[0] : Resources.UnknownRouteSection : Resources.UnknownRouteSection;
            var application = routeValues.Count() > 1 ? routeValues[1] : Resources.UnknownRouteSection;
            var documentType = routeValues.Count() > 2 ? routeValues[2] : Resources.UnknownRouteSection;
            var service = routeValues.Count() > 3 ? routeValues[3] : Resources.UnknownRouteSection;
            var instanceId = routeValues.Count() > 4 ? Guid.TryParse(routeValues[4],out instanceGuid) ? routeValues[4] : Resources.UnknownRouteSection : Resources.UnknownRouteSection;

            return new Dictionary<string, string>()
            {
                {"version",version},
                {"application",application},
                {"documentType",documentType},                
                {"service",service},
                {"instanceId",instanceId},
            };
        }

        public PathString FormatRoutePath(IOwinContext context, PathString path)
        {
            var routeDictionary = GetRouteDictionary(context);

            return new PathString(path.HasValue
                ? path.Value
                    .ReplaceFormat("_version_",routeDictionary["version"])
                    .ReplaceFormat("_application_", routeDictionary["application"])
                    .ReplaceFormat("_documentType_", routeDictionary["documentType"])
                    .ReplaceFormat("_service_", routeDictionary["service"])
                    .ReplaceFormat("_instanceId_", routeDictionary["instanceId"]) : string.Empty); 

        }
    }
}
