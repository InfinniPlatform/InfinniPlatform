using System.Threading.Tasks;
using InfinniPlatform.DocumentStorage;
using InfinniPlatform.SandboxApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.SandboxApp
{
    [Route("test")]
    [ApiExplorerSettings(IgnoreApi = false,GroupName = nameof(HttpController))]
    public class HttpController : Controller
    {
        private readonly IDocumentStorageProvider<Entity> _documentStorageProvider;

        public HttpController(IDocumentStorageProviderFactory storageFactory)
        {
            _documentStorageProvider = storageFactory.GetStorageProvider<Entity>();

        }

        [HttpPost("save")]
        public async Task<object> Save()
        {
            var doc = new Entity {Digit = 5, Name = "Five"};

            await _documentStorageProvider.InsertOneAsync(doc);

            return doc;
        }

        [HttpGet("get")]
        public async Task<object> Get()
        {
            var foo = await _documentStorageProvider.Find().Limit(10).ToListAsync();

            return foo;
        }
    }
}