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
    public sealed class RouteFormatterAuthUser : IRouteFormatter
    {
        /// <summary>
        ///  Заполнить список значений роутинга регистрации
        /// </summary>
        /// <param name="context">Контекст выполнения запросов</param>
        /// <returns></returns>
        public Dictionary<string, string> GetRouteDictionary(IOwinContext context)
        {
            var routeValues = context.Request.Path.HasValue
                ? context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

            int versionInt = 0;

            var version = routeValues.Count() > 0 ? Int32.TryParse(routeValues[0], out versionInt) ? routeValues[0] : Resources.UnknownRouteSection : Resources.UnknownRouteSection;

            var application = routeValues.Count() > 1 ? routeValues[1] : Resources.UnknownRouteSection;
            var documentType = routeValues.Count() > 2 ? routeValues[2] : Resources.UnknownRouteSection;
            var userName = routeValues.Count() > 3 ? routeValues[3] : Resources.UnknownRouteSection;
            var roleName = routeValues.Count() > 5 ? routeValues[5] : Resources.UnknownRouteSection;
            var claimType = routeValues.Count() > 5 ? routeValues[5] : Resources.UnknownRouteSection;

            return new Dictionary<string, string>()
            {
                {"version",version},
                {"application",application},
                {"documentType",documentType},                
                {"userName", userName},
                {"roleName", roleName},
                {"claimType",claimType},                
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
                    .ReplaceFormat("_userName_", routeDictionary["userName"])
                    .ReplaceFormat("_roleName_", routeDictionary["roleName"])
                    .ReplaceFormat("_claimType_", routeDictionary["claimType"])
                    : string.Empty);
        }
    }
}
