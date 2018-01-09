using System.Net;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Http
{
    /// <summary>
    /// Extension methods for <see cref="HttpContext" />.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Checks if request is local.
        /// </summary>
        /// <param name="httpContext">Instance of <see cref="HttpContext" />.</param>
        /// <returns>Return <c>true</c>, if address of the requester if local, else - <c>false</c>.</returns>
        public static bool IsLocal(this HttpContext httpContext)
        {
            var connection = httpContext.Connection;
            if (connection.RemoteIpAddress != null)
            {
                return connection.LocalIpAddress != null
                           ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                           : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            // for in memory TestServer or when dealing with default connection info
            if (connection.RemoteIpAddress == null && connection.LocalIpAddress == null)
            {
                return true;
            }

            return false;
        }
    }
}