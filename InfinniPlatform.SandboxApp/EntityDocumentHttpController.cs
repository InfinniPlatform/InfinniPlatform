using System.Threading.Tasks;
using InfinniPlatform.DocumentStorage;
using InfinniPlatform.Logging;
using InfinniPlatform.SandboxApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.SandboxApp
{
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(DocumentHttpController))]
    public class DocumentHttpController : Controller
    {
        // GET
        [HttpGet("/documents/Entity/{id}")]
        public Entity Get(string id)
        {
            return new Entity();
        }

        [HttpGet("/documents/Entity/")]
        public Entity Get()
        {
            return new Entity();
        }

        [HttpPost("/documents/Entity/")]
        public Entity Post([FromBody] Entity document)
        {
            return new Entity();
        }

        [HttpDelete("/documents/Entity/{id}")]
        public Entity Delete(string id)
        {
            return new Entity();
        }

        [HttpDelete("/documents/Entity/")]
        public Entity Delete()
        {
            return new Entity();
        }
    }

    public class EntityDocumentHttpController : DocumentControllerBase
    {
        public EntityDocumentHttpController(IPerformanceLogger perfLogger, ILogger logger) : base(perfLogger, logger)
        {
        }

        protected override Task<object> Get()
        {
            return Task.FromResult<object>(new Entity {Name = "1", Digit = 1});
        }

        protected override Task<object> Post()
        {
            return Task.FromResult<object>(new Entity { Name = "1", Digit = 1 });
        }

        protected override Task<object> Delete()
        {
            return Task.FromResult<object>(new Entity { Name = "1", Digit = 1 });
        }
    }
}