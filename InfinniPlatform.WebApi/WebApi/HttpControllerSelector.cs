using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                      .Where(i => !i.IsDynamic
                                                  && !string.IsNullOrEmpty(i.Location)
                                                  && i.FullName.StartsWith("Infinni"))
                                      .ToArray();

            var result = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

            foreach (var assembly in assemblies)
            {
                try
                {
                    var controllerTypes = assembly.GetTypes().Where(t => !t.IsAbstract && t.Name.EndsWith(ControllerSuffix) && typeof(ApiController).IsAssignableFrom(t)).ToArray();

                    if (controllerTypes.Length > 0)
                    {
                        foreach (var controllerType in controllerTypes)
                        {
                            if (!result.ContainsKey(controllerType.Name))
                            {
                                result.Add(controllerType.Name, controllerType);
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            return result;
        }


        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllerName = GetControllerName(request);

            Type controllerType;

            _controllerTypes.Value.TryGetValue(controllerName + ControllerSuffix, out controllerType);

            return new HttpControllerDescriptor(_configuration, controllerName, controllerType);
        }
    }
}