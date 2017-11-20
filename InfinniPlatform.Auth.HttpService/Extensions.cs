using System.Security.Principal;

using InfinniPlatform.Http;

using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.Auth.HttpService
{
    public static class Extensions
    {
        public const string ApplicationAuthScheme = "Identity.Application";
        public const string ExternalAuthScheme = "Identity.External";

        public static bool IsAuthenticated(this IIdentity identity)
        {
            return identity != null && identity.IsAuthenticated;
        }

        public static JsonResult CreateErrorResponse(string errorMessage, int statusCode)
        {
            var errorResponse = new JsonResult(new ServiceResult<object> { Success = false, Error = errorMessage }) { StatusCode = statusCode };
            return errorResponse;
        }

        public static JsonResult CreateSuccesResponse<TResult>(TResult result) where TResult : class
        {
            var successResponse = new JsonResult(new ServiceResult<TResult> { Success = true, Result = result }) { StatusCode = 200 };
            return successResponse;
        }
    }
}