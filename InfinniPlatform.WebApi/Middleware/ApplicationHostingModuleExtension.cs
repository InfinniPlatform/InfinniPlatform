using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfinniPlatform.Api.Properties;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware
{
    /// <summary>
    ///   ������ ���������� ��� ������ � ��������� ����������
    /// </summary>
    public static class ApplicationHostingModuleExtension
    {
        public static Dictionary<string, string> GetRouteDictionary(this IOwinContext context)
        {
            var routeValues = context.Request.Path.HasValue
                ? context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };
            int versionNumber = -1;

            var versionApp = routeValues.Any() ? int.TryParse(routeValues[0], out versionNumber) ? routeValues[0] : Resources.UnsatisfiedVersionNumber : string.Empty;
            var application = routeValues.Count() > 1 ? routeValues[1] : string.Empty;
            var documentType = routeValues.Count() > 2 ? routeValues[2] : string.Empty;
            var service = routeValues.Count() > 3 ? routeValues[3] : string.Empty;

            return new Dictionary<string, string>()
            {
                {"version",versionApp },
                {"documentType",documentType},
                {"application",application},
                {"service",service}
            };
        } 

        /// <summary>
        ///   �������� �������� �������� �� ��������� �������
        /// </summary>
        /// <param name="context">�������� �������</param>
        /// <param name="path">������� �������</param>
        /// <returns>�������� ��������� ��������</returns>
        public static PathString FormatRoutePath(this IOwinContext context, PathString path)
        {
            var routeDictionary = context.GetRouteDictionary();

            return new PathString( path.HasValue
                ? path.Value
                    .ReplaceFormat("_version_", routeDictionary["version"])
                    .ReplaceFormat("_application_", routeDictionary["application"])
                    .ReplaceFormat("_documentType_", routeDictionary["documentType"])
                    .ReplaceFormat("_service_", routeDictionary["service"]) : string.Empty);
        }

        private static string ReplaceFormat(this string processingString, string oldString, string newString)
        {
            return processingString.Replace(oldString, newString);
        }       
    }
}