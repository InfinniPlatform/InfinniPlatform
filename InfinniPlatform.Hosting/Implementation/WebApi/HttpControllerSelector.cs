using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace InfinniPlatform.Hosting.WebApi.Implementation.WebApi
{
	public sealed class HttpControllerSelector : DefaultHttpControllerSelector
	{
		private readonly HttpConfiguration _configuration;

		public HttpControllerSelector(HttpConfiguration configuration) : base(configuration)
		{
			_configuration = configuration;
		}

		private IEnumerable<KeyValuePair<string, Type>> _apiControllerTypes;

		private IEnumerable<KeyValuePair<string, Type>> ApiControllerTypes
		{
			get { return _apiControllerTypes ?? (_apiControllerTypes = GetControllerTypes()); }
		}

		private static IEnumerable<KeyValuePair<string, Type>> GetControllerTypes()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			var types = assemblies.SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && t.Name.EndsWith(ControllerSuffix) && typeof(IHttpController).IsAssignableFrom(t)))
				.Select(t => new KeyValuePair<string, Type>(t.FullName,t));

			return types;
		}

		public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
		{
			return new HttpControllerDescriptor(_configuration, GetControllerName(request),
			                                    ApiControllerTypes.Where(a => a.Key.Contains(GetControllerName(request))).Select(a => a.Value).FirstOrDefault());
		}
	}
}