using System.Collections.Generic;
using System.Reflection;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace InfinniPlatform.ServiceHost
{
    public class GenericControllerApplicationPart : ApplicationPart, IApplicationPartTypeProvider
    {
        public GenericControllerApplicationPart(IEnumerable<TypeInfo> types)
        {
            Types = types;
        }

        public override string Name => "GenericController";
        public IEnumerable<TypeInfo> Types { get; }
    }

    public class DocumentHttpController<T> : Controller where T : Document, new()
    {
        // GET
        [HttpGet("/documents/Entity/{id}")]
        public T Get(string id)
        {
            return new T();
        }

        [HttpGet("/documents/Entity/")]
        public T Get()
        {
            return new T();
        }

        [HttpPost("/documents/Entity/")]
        public T Post([FromBody] T document)
        {
            return new T();
        }

        [HttpDelete("/documents/Entity/{id}")]
        public T Delete(string id)
        {
            return new T();
        }

        [HttpDelete("/documents/Entity/")]
        public T Delete()
        {
            return new T();
        }
    }
}