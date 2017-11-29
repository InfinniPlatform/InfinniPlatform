using InfinniPlatform.Http;

using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// Extensions for <see cref="InfinniPlatform.Auth.HttpService"/> controllers.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Internal authentication scheme name. 
        /// </summary>
        public const string ApplicationAuthScheme = "Identity.Application";
        
        /// <summary>
        /// External authentication scheme name. 
        /// </summary>
        public const string ExternalAuthScheme = "Identity.External";

        /// <summary>
        /// Creates response for successful request. 
        /// </summary>
        /// <param name="result">Result object.</param>
        public static JsonResult CreateSuccesResponse(object result = null)
        {
            return new JsonResult(new ServiceResult<object> { Success = true, Result = result }) { StatusCode = 200 };
        }

        /// <summary>
        /// Creates response for failed request. 
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        /// <param name="statusCode">Error HTTP status code.</param>
        public static JsonResult CreateErrorResponse(string errorMessage, int statusCode)
        {
            return new JsonResult(new ServiceResult<object> { Success = false, Error = errorMessage }) { StatusCode = statusCode };
        }
    }
}