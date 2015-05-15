using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfinniPlatform.Api.Properties;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware
{
    /// <summary>
    ///   Методы расширения для работы с хостингом приложений
    /// </summary>
    public static class ApplicationHostingModuleExtension
    {
        public static Dictionary<string, string> GetSessionRouteDictionary(this IOwinContext context)
        {
            var routeValues = context.Request.Path.HasValue
                ? context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

            int versionNumber = -1;

            var versionApp = routeValues.Any() ? int.TryParse(routeValues[0], out versionNumber) ? routeValues[0] : Resources.UnsatisfiedVersionNumber : string.Empty;
            var sessionId = routeValues.Count() > 1 ? routeValues[1] : string.Empty;
            var attachmentId = routeValues.Count() > 2 ? routeValues[2] : string.Empty;

            return new Dictionary<string, string>()
            {
                {"version",versionApp },
                {"sessionId",sessionId},
                {"attachmentId",attachmentId}
            };
        }

        public static Dictionary<string, string> GetRouteDictionary(this IOwinContext context)
        {
            var routeValues = context.Request.Path.HasValue
                ? context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };
            int versionNumber = -1;

            var versionApp = routeValues.Any() ? int.TryParse(routeValues[0], out versionNumber) ? routeValues[0] : Resources.UnsatisfiedVersionNumber : string.Empty;
            var application = routeValues.Count() > 1 ? routeValues[1] : "Unknown";
            var documentType = routeValues.Count() > 2 ? routeValues[2] : "Unknown";
            var service = routeValues.Count() > 3 ? routeValues[3] : "Unknown";
            var instanceId = routeValues.Count() > 3 ? routeValues[3] : "Unknown";

            return new Dictionary<string, string>()
            {
                {"version",versionApp },
                {"application",application},
                {"documentType",documentType},                
                {"service",service},
                {"instanceId",instanceId}
            };
        } 

        /// <summary>
        ///   Получить параметр роутинга из контекста запроса
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <param name="path">Роутинг запроса</param>
        /// <returns>Значение параметра роутинга</returns>
        public static PathString FormatRoutePath(this IOwinContext context, PathString path)
        {
            var routeDictionary = context.GetRouteDictionary();

            return new PathString( path.HasValue
                ? path.Value
                    .ReplaceFormat("_version_", routeDictionary["version"])
                    .ReplaceFormat("_application_", routeDictionary["application"])
                    .ReplaceFormat("_documentType_", routeDictionary["documentType"])
                    .ReplaceFormat("_service_", routeDictionary["service"]) 
                    .ReplaceFormat("_instanceId_", routeDictionary["instanceId"]) : string.Empty);
        }

        public static PathString FormatSessionRoutePath(this IOwinContext context, PathString path)
        {
            var routeDictionary = context.GetSessionRouteDictionary();

            return new PathString(path.HasValue
                ? path.Value
                    .ReplaceFormat("_version_", routeDictionary["version"])
                    .ReplaceFormat("_sessionId_", routeDictionary["sessionId"])
                    .ReplaceFormat("_attachmentId_", routeDictionary["attachmentId"]) 
                    : string.Empty);
        }

        private static string ReplaceFormat(this string processingString, string oldString, string newString)
        {
            return processingString.Replace(oldString, newString);
        }       
    }
}