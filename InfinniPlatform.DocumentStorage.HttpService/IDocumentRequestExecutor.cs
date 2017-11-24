using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Processes request to document controller.
    /// </summary>
    public interface IDocumentRequestExecutor
    {
        /// <summary>
        /// Process GET request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="routeData"></param>
        Task<object> Get(HttpRequest request, RouteData routeData);

        /// <summary>
        /// Process POST request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="routeData"></param>
        Task<object> Post(HttpRequest request, RouteData routeData);

        /// <summary>
        /// Process DELETE request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="routeData"></param>
        Task<object> Delete(HttpRequest request, RouteData routeData);
    }
}