using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var routeValues = context.Request.Path.HasValue
    ? context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

            var application = routeValues.Count() > 0 ? routeValues[0] : Resources.UnknownRouteSection;
            var documentType = routeValues.Count() > 1 ? routeValues[1] : Resources.UnknownRouteSection;
            var service = routeValues.Count() > 2 ? routeValues[2] : Resources.UnknownRouteSection;
            var instanceId = routeValues.Count() > 3 ? Guid.TryParse(routeValues[3],out instanceGuid) ? routeValues[3] : Resources.UnknownRouteSection : Resources.UnknownRouteSection;

            return new Dictionary<string, string>()
            {
                {"application",application},
                {"documentType",documentType},                
                {"service",service},
                {"instanceId",instanceId}
            };
        }

        public PathString FormatRoutePath(IOwinContext context, PathString path)
        {
            var routeDictionary = GetRouteDictionary(context);

            return new PathString(path.HasValue
                ? path.Value
                    .ReplaceFormat("_application_", routeDictionary["application"])
                    .ReplaceFormat("_documentType_", routeDictionary["documentType"])
                    .ReplaceFormat("_service_", routeDictionary["service"])
                    .ReplaceFormat("_instanceId_", routeDictionary["instanceId"] ) : string.Empty);
        }
    }
}
