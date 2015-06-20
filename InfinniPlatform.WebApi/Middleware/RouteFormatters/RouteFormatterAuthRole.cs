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
    public sealed class RouteFormatterAuthRole : IRouteFormatter
    {
        public Dictionary<string, string> GetRouteDictionary(IOwinContext context)
        {
            var routeValues = context.Request.Path.HasValue
                ? context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

            int versionNumber = -1;

            var versionApp = routeValues.Any() ? int.TryParse(routeValues[0], out versionNumber) ? routeValues[0] : Resources.UnsatisfiedVersionNumber : Resources.UnknownRouteSection;
            var application = routeValues.Count() > 1 ? routeValues[1] : Resources.UnknownRouteSection;
            var documentType = routeValues.Count() > 2 ? routeValues[2] : Resources.UnknownRouteSection;
            var roleName = routeValues.Count() > 3 ? routeValues[3] : Resources.UnknownRouteSection;
            return new Dictionary<string, string>()
            {
                {"version",versionApp },
                {"application",application},
                {"documentType",documentType},                
                {"roleName", roleName}
            };
        }

        public PathString FormatRoutePath(IOwinContext context, PathString path)
        {
            var routeDictionary = GetRouteDictionary(context);

            return new PathString(path.HasValue
                ? path.Value
                    .ReplaceFormat("_version_", routeDictionary["version"])
                    .ReplaceFormat("_application_", routeDictionary["application"])
                    .ReplaceFormat("_documentType_", routeDictionary["documentType"])
                    .ReplaceFormat("_roleName_", routeDictionary["roleName"])
                    : string.Empty);
        }
    }
}
