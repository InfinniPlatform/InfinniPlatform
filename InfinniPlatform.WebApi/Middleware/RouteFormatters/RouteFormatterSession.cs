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

            var sessionId = routeValues.Count() > 0 ? routeValues[0] : Resources.UnknownRouteSection;
            var attachmentId = routeValues.Count() > 1 ? routeValues[1] : Resources.UnknownRouteSection;

            return new Dictionary<string, string>()
            {
               {"sessionId",sessionId},
                {"attachmentId",attachmentId}
            };
        }

        public PathString FormatRoutePath(IOwinContext context, PathString path)
        {
            var routeDictionary = GetRouteDictionary(context);

            return new PathString(path.HasValue
                ? path.Value
                    .ReplaceFormat("_sessionId_", routeDictionary["sessionId"])
                    .ReplaceFormat("_attachmentId_", routeDictionary["attachmentId"])
                    : string.Empty);
        }
    }
}
