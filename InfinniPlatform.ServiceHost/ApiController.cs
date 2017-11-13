using System.Linq;
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
            return new JsonResult(_provider.ApiDescriptionGroups.Items.Select(i=> new ControllerDescription(i)));
        }
    }

    public class ControllerDescription
    {
        public ControllerDescription(ApiDescriptionGroup apiDescriptionGroup)
        {
            ControllerName = apiDescriptionGroup.GroupName;
            Actions = apiDescriptionGroup.Items.Select(i => new ActionDescription(i)).ToArray();
        }

        public string ControllerName { get; set; }
        public ActionDescription[] Actions { get; set; }

        public class ActionDescription
        {
            public ActionDescription(ApiDescription description)
            {
                HttpMethod = description.HttpMethod;
                Path = description.RelativePath;
                ParametersDescription = description.ParameterDescriptions.Select(d => new ParametersDescription(d)).ToArray();
            }

            public string HttpMethod { get; }
            public string Path { get; }
            public ParametersDescription[] ParametersDescription { get; }
        }

        public class ParametersDescription
        {
            public ParametersDescription(ApiParameterDescription description)
            {
                Name = description.Name;
                Type = description.Type.FullName;
            }

            public string Name { get; }
            public string Type { get; }
        }
    }
}