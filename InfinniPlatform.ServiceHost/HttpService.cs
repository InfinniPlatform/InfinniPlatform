using System.Threading.Tasks;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace InfinniPlatform.ServiceHost
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly IApiDescriptionGroupCollectionProvider _provider;

        public ApiController(IApiDescriptionGroupCollectionProvider provider)
        {
            _provider = provider;
        }

        [HttpGet("list")]
        public JsonResult Api()
        {
            return new JsonResult(_provider.ApiDescriptionGroups);
        }
    }

    [Route("test")]
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