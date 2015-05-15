using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace InfinniPlatform.WebApi.WebApi
{
	public sealed class HttpControllerSelector : DefaultHttpControllerSelector
	{
		private readonly HttpConfiguration _configuration;

		public HttpControllerSelector(HttpConfiguration configuration) : base(configuration)
		{
			_configuration = configuration;
		}

		private static IEnumerable<KeyValuePair<string, Type>> _apiControllerTypes;

		private IEnumerable<KeyValuePair<string, Type>> ApiControllerTypes
		{
			get { return _apiControllerTypes ?? (_apiControllerTypes = GetControllerTypes()); }
		}
        
		private static IEnumerable<KeyValuePair<string, Type>> GetControllerTypes()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies().Distinct();

		    var result = new List<KeyValuePair<string, Type>>();

		    foreach (var assembly in assemblies)
		    {
		        try
		        {
		            var applicableTypes = assembly.GetTypes()
		                .Where(t =>
		                    !t.IsAbstract && t.Name.EndsWith(ControllerSuffix) &&
		                    typeof (IHttpController).IsAssignableFrom(t));

		            result.AddRange(applicableTypes.Select(applicableType => new KeyValuePair<string, Type>(applicableType.FullName, applicableType)));
		        }
		        catch
		        {
		            // Некоторые сборки (например, Microsoft.Owin.Security) содержат классы, заканчивающиеся на ControllerSuffix, но
		            // загрузка этих типов приводит к исключениям ввиду отсутствия зависимых сборок. Пропускаем подобные сборки.
		        }
		    }

		    return result;
		}

        public static void ClearCache()
	    {
	        _apiControllerTypes = null;
	    }

		public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
		{
			var controllerType =
				ApiControllerTypes.FirstOrDefault(a => a.Key.ToLowerInvariant().Contains((GetControllerName(request)).ToLowerInvariant() + "controller"));
			return new HttpControllerDescriptor(_configuration, GetControllerName(request),controllerType.Value);
		}
	}
}