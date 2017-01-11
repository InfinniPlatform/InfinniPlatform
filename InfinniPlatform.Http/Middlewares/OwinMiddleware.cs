using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// An abstract base class for a standard middleware pattern.
    /// </summary>
    public abstract class OwinMiddleware
    {
        /// <summary>The optional next component.</summary>
        protected OwinMiddleware Next { get; set; }

        /// <summary>
        /// Instantiates the middleware with an optional pointer to the next component.
        /// </summary>
        /// <param name="next"></param>
        protected OwinMiddleware(OwinMiddleware next)
        {
            this.Next = next;
        }

        /// <summary>Process an individual request.</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task Invoke(HttpContext context);
    }
}