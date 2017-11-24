using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Базовый класс сервисов по работе с документами.
    /// </summary>
    [Route("documents")]
    public class DocumentsController : Controller
    {
        public DocumentsController(IDocumentRequestExecutorProvider processorProvider)
        {
            _processorProvider = processorProvider;
        }


        private readonly IDocumentRequestExecutorProvider _processorProvider;

        [HttpGet("{documentType}/{id?}")]
        public async Task<object> ProcessGet(string documentType, string id)
        {
            return await _processorProvider.Get(documentType).Get(Request, RouteData);
        }

        [HttpPost("{documentType}")]
        public async Task<object> ProcessPost(string documentType)
        {
            return await _processorProvider.Get(documentType).Post(Request, RouteData);
        }

        [HttpDelete("{documentType}/{id?}")]
        public async Task<object> ProcessDelete(string documentType, string id)
        {
            return await _processorProvider.Get(documentType).Delete(Request, RouteData);
        }
    }
}