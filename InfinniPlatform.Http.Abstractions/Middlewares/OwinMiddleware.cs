using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// An abstract base class for a standard middleware pattern.
    /// </summary>
    public abstract class OwinMiddleware
    {
        /// <summary>
        /// Instantiates the middleware with an optional pointer to the next component.
        /// </summary>
        protected OwinMiddleware(RequestDelegate next)
        {
            Next = next;
        }


        /// <summary>
        /// A function that can process an HTTP request after this middleware.
        /// </summary>
        protected RequestDelegate Next { get; set; }


        /// <summary>
        /// Process an individual request.
        /// </summary>
        public abstract Task Invoke(HttpContext context);
    }
}