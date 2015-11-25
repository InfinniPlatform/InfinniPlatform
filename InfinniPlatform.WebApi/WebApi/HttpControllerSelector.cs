using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace InfinniPlatform.WebApi.WebApi
{
    internal sealed class HttpControllerSelector : DefaultHttpControllerSelector
    {
        public HttpControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            _controllerTypes = new Lazy<Dictionary<string, Type>>(GetControllerTypes);
        }


        private readonly HttpConfiguration _configuration;
        private readonly Lazy<Dictionary<string, Type>> _controllerTypes;


        private static Dictionary<string, Type> GetControllerTypes()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            var result = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

            foreach (var type in currentAssembly.GetTypes())
            {
                if (!type.IsAbstract
                    && type.Name.EndsWith(ControllerSuffix)
                    && typeof(ApiController).IsAssignableFrom(type))
                {
                    var controllerName = type.Name.Substring(0, type.Name.Length - ControllerSuffix.Length);

                    if (!result.ContainsKey(controllerName))
                    {
                        result.Add(controllerName, type);
                    }
                }
            }

            return result;
        }


        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllerName = GetControllerName(request);

            Type controllerType;

            _controllerTypes.Value.TryGetValue(controllerName, out controllerType);

            return new HttpControllerDescriptor(_configuration, controllerName, controllerType);
        }
    }
}