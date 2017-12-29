using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Provides HTTP API for document storage.
    /// </summary>
    [Route("documents")]
    public class DocumentsController : Controller
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DocumentsController" />.
        /// </summary>
        /// <param name="processorProvider">Provider of <see cref="DocumentRequestExecutor"/> instances.</param>
        public DocumentsController(IDocumentRequestExecutorProvider processorProvider)
        {
            _processorProvider = processorProvider;
        }


        private readonly IDocumentRequestExecutorProvider _processorProvider;

        /// <summary>
        /// Returns document by identifier.
        /// </summary>
        /// <param name="documentType">Docuemnt type.</param>
        /// <param name="id">Document identifier.</param>
        [HttpGet("{documentType}/{id?}")]
        public async Task<object> ProcessGet(string documentType, string id)
        {
            return await _processorProvider.Get(documentType).Get(Request, RouteData);
        }

        /// <summary>
        /// Saves document.
        /// </summary>
        /// <param name="documentType">Docuemnt type.</param>
        [HttpPost("{documentType}")]
        public async Task<object> ProcessPost(string documentType)
        {
            return await _processorProvider.Get(documentType).Post(Request, RouteData);
        }

        /// <summary>
        /// Deletes document by identifier.
        /// </summary>
        /// <param name="documentType">Docuemnt type.</param>
        /// <param name="id">Document identifier.</param>
        [HttpDelete("{documentType}/{id?}")]
        public async Task<object> ProcessDelete(string documentType, string id)
        {
            return await _processorProvider.Get(documentType).Delete(Request, RouteData);
        }
    }
}