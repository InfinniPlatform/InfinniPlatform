using InfinniPlatform.ServiceHost.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.ServiceHost
{
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(EntityDocumentHttpController))]
    public class EntityDocumentHttpController : Controller
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
}