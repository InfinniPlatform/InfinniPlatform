using System.Security.Principal;
using InfinniPlatform.Http;

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

        public static JsonHttpResponse CreateErrorResponse(string errorMessage, int statusCode)
        {
            var errorResponse = new JsonHttpResponse(new ServiceResult<object> {Success = false, Error = errorMessage}) {StatusCode = statusCode};
            return errorResponse;
        }

        public static JsonHttpResponse CreateSuccesResponse<TResult>(TResult result) where TResult : class
        {
            var successResponse = new JsonHttpResponse(new ServiceResult<TResult> {Success = true, Result = result}) {StatusCode = 200};
            return successResponse;
        }
    }
}