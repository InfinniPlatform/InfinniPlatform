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
            var application = routeValues.Count() > 1 ? routeValues[1] : Resources.UnknownRouteSection;
            var documentType = routeValues.Count() > 2 ? routeValues[2] : Resources.UnknownRouteSection;
            var service = routeValues.Count() > 3 ? routeValues[3] : Resources.UnknownRouteSection;
            var instanceId = routeValues.Count() > 3 ? routeValues[3] : Resources.UnknownRouteSection;

            return new Dictionary<string, string>()
            {
                {"version",versionApp },
                {"application",application},
                {"documentType",documentType},                
                {"service",service},
                {"instanceId",instanceId}
            };
        }

        public static Dictionary<string, string> GetAccessRouteDictionary(this IOwinContext context)
        {
            var routeValues = context.Request.Path.HasValue
                ? context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

            int versionNumber = -1;

            var versionApp = routeValues.Any() ? int.TryParse(routeValues[0], out versionNumber) ? routeValues[0] : Resources.UnsatisfiedVersionNumber : string.Empty;
            var application = routeValues.Count() > 1 ? routeValues[1] : Resources.UnknownRouteSection;
            var documentType = routeValues.Count() > 2 ? routeValues[2] : Resources.UnknownRouteSection;
            var service = routeValues.Count() > 3 ? routeValues[3] : Resources.UnknownRouteSection;
            var instanceId = routeValues.Count() > 4 ? routeValues[4] : Resources.UnknownRouteSection;
            var userName = routeValues.Count() > 5 ? routeValues[5] : Resources.UnknownRouteSection;

            return new Dictionary<string, string>()
            {
                {"version",versionApp },
                {"application",application},
                {"documentType",documentType},                
                {"service",service},
                {"instanceId",instanceId},
                {"userName", userName}
            };
        } 

        /// <summary>
        ///   Форматировать шаблон роутинга запроса, используя параметры запроса
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <param name="path">Шаблон роутинга запроса</param>
        /// <returns>Форматированный роутинг запроса</returns>
        public static PathString FormatRoutePath(this IOwinContext context, PathString path)
        {
            var routeDictionary = context.GetRouteDictionary();

            Guid guid = Guid.NewGuid();
            bool isInstanceRoute = Guid.TryParse(routeDictionary["instanceId"], out guid);


            return new PathString(path.HasValue
                ? path.Value
                    .ReplaceFormat("_version_", routeDictionary["version"])
                    .ReplaceFormat("_application_", routeDictionary["application"])
                    .ReplaceFormat("_documentType_", routeDictionary["documentType"])
                    .ReplaceFormat("_service_", routeDictionary["service"])
                    .ReplaceFormat("_instanceId_", isInstanceRoute ? routeDictionary["instanceId"] : string.Empty) :string.Empty);
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