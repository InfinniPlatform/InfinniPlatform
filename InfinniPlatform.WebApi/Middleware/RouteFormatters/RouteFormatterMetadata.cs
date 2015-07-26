using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Properties;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.RouteFormatters
{
    public class RouteFormatterMetadata : IRouteFormatter
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


            var application = routeValues.Count() > 0 ? routeValues[0] : Resources.UnknownRouteSection;
            var documentType = routeValues.Count() > 1 ? routeValues[1] : Resources.UnknownRouteSection;
            var version = routeValues.Count() > 2 ? routeValues[2] : Resources.UnknownRouteSection;
            var instanceId = routeValues.Count() > 3 ? routeValues[3] : Resources.UnknownRouteSection;
            
            return new Dictionary<string, string>()
            {
                {"application",application},
                {"documentType",documentType},                
                {"version", version},
                {"instanceId", instanceId}
            };
        }

        public PathString FormatRoutePath(IOwinContext context, PathString path)
        {
            var routeDictionary = GetRouteDictionary(context);

            return new PathString(path.HasValue
                ? path.Value
                    .ReplaceFormat("_application_", routeDictionary["application"])
                    .ReplaceFormat("_documentType_", routeDictionary["documentType"])
                    .ReplaceFormat("_version_", routeDictionary["version"])
                    .ReplaceFormat("_instanceId_", routeDictionary["instanceId"])
                    : string.Empty);
        }
    }
}
