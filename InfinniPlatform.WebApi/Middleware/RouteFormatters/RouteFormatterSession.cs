using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Owin.Middleware;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.RouteFormatters
{
    public sealed class RouteFormatterSession : IRouteFormatter
    {
        public Dictionary<string, string> GetRouteDictionary(IOwinContext context)
        {
            var routeValues = context.Request.Path.HasValue
    ? context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

            int versionNumber = -1;

            int versionInt = 0;

            var version = routeValues.Count() > 0 ? Int32.TryParse(routeValues[0], out versionInt) ? routeValues[0] : Resources.UnknownRouteSection : Resources.UnknownRouteSection;
            var sessionId = routeValues.Count() > 1 ? routeValues[1] : Resources.UnknownRouteSection;
            var attachmentId = routeValues.Count() > 2 ? routeValues[2] : Resources.UnknownRouteSection;

            return new Dictionary<string, string>()
            {
                {"version",version},
                {"sessionId", sessionId},
                {"attachmentId", attachmentId}
            };
        }

        public PathString FormatRoutePath(IOwinContext context, PathString path)
        {
            var routeDictionary = GetRouteDictionary(context);

            return new PathString(path.HasValue
                ? path.Value
                    .ReplaceFormat("_version_", routeDictionary["version"])
                    .ReplaceFormat("_sessionId_", routeDictionary["sessionId"])
                    .ReplaceFormat("_attachmentId_", routeDictionary["attachmentId"])
                    : string.Empty);
        }
    }
}
