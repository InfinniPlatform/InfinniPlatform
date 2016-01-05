using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Core.Properties;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.RouteFormatters
{
    public class RouteFormatterMetadataDocumentElement : IRouteFormatter
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
            var versionMetadata = routeValues.Count() > 2 ? routeValues[2] : Resources.UnknownRouteSection;
            var configuration = routeValues.Count() > 3 ? routeValues[3] : Resources.UnknownRouteSection;
            var document = routeValues.Count() > 4 ? routeValues[4] : Resources.UnknownRouteSection;            
            var metadataType = routeValues.Count() > 5 ? routeValues[5] : Resources.UnknownRouteSection;
            var instanceId = routeValues.Count() > 6 ? routeValues[6] : Resources.UnknownRouteSection;
            
            return new Dictionary<string, string>()
            {
                {"application",application},
                {"version", version},
                {"versionMetadata", versionMetadata},
                {"configuration",configuration},
                {"document",document},
                {"metadataType",metadataType},                 
                {"instanceId", instanceId}
            };
        }

        public PathString FormatRoutePath(IOwinContext context, PathString path)
        {
            var routeDictionary = GetRouteDictionary(context);

            return new PathString(path.HasValue
                ? path.Value
                    .ReplaceFormat("_application_", routeDictionary["application"])
                    .ReplaceFormat("_metadataType_", routeDictionary["metadataType"])
                    .ReplaceFormat("_configuration_", routeDictionary["configuration"])
                    .ReplaceFormat("_document_",routeDictionary["document"])
                    .ReplaceFormat("_version_", routeDictionary["version"])
                    .ReplaceFormat("_versionMetadata_", routeDictionary["versionMetadata"])
                    .ReplaceFormat("_instanceId_", routeDictionary["instanceId"])
                    : string.Empty);
        }
    }
}
