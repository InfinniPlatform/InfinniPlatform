using System.Threading.Tasks;
using InfinniPlatform.DocumentStorage;
using InfinniPlatform.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.ServiceHost
{
    public class HttpController : Controller
    {
        private readonly IDocumentStorageProvider<Entity> _documentStorageProvider;

        public HttpController(IDocumentStorageProviderFactory storageFactory)
        {
            _documentStorageProvider = storageFactory.GetStorageProvider<Entity>();
        }

        [HttpPost("/save")]
        public async Task<object> Save()
        {
            var doc = new Entity {Digit = 5, Name = "Five"};

            await _documentStorageProvider.InsertOneAsync(doc);

            return doc;
        }

        [HttpGet("/get")]
        public async Task<object> Get()
        {
            var foo = await _documentStorageProvider.Find().Limit(10).ToListAsync();

            return foo;
        }
    }

    public class Entity : Document
    {
        public Entity()
        {
            Name = "Default";
            Digit = 1;
        }

        public string Name { get; set; }
        public int Digit { get; set; }
    }
}