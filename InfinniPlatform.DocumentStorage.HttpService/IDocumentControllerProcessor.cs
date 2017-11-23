using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace InfinniPlatform.DocumentStorage
{
    public interface IDocumentControllerProcessor
    {
        Task<object> Get(HttpRequest request, RouteData routeData);

        Task<object> Post(HttpRequest request, RouteData routeData);

        Task<object> Delete(HttpRequest request, RouteData routeData);
    }
}