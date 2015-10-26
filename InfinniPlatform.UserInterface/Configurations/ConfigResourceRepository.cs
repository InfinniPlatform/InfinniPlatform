using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Settings;
using InfinniPlatform.UserInterface.Services.Metadata;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.UserInterface.Configurations
{
	internal static class ConfigResourceRepository
	{
		static readonly Assembly Assembly;
		static readonly string[] Resources;
		
		static ConfigResourceRepository()
		{
			Assembly = Assembly.GetCallingAssembly();
			Resources = Assembly.GetManifestResourceNames();
		}

		// Menu

		public static object GetMenu(string configId, string menuId)
		{
			var resource = string.Format(@"{0}.Configurations.{1}.Menu.{2}.resjson", Assembly.GetName().Name, configId,
										 menuId);
			return ReadObject(resource);
		}

		public static IEnumerable GetMenu(string configId)
		{
			var resource = string.Format(@"{0}.Configurations.{1}.Menu.", Assembly.GetName().Name, configId);
			var items = Resources.Where(r => r.StartsWith(resource)).Select(ReadObject).ToArray();
			return (items.Length > 0)
					   ? items
					   : null;
		}

		// View

		public static object GetView(string configId, string documentId, string viewId)
		{
			if (configId == "Designer")
			{
				var resource = string.Format(@"{0}.Configurations.{1}.Documents.{2}.{3}.resjson", Assembly.GetName().Name, configId, documentId, viewId);
				return ReadObject(resource);
			}

			var configPath = AppSettings.GetValue("ContentDirectory", "..\\Assemblies\\content");

			var filePath = Path.Combine(configPath, "Documents", documentId, "Views", string.Format("{0}.json", viewId));

			var loadItemMetadata = PackageMetadataLoader.LoadItemMetadata(filePath);

			return loadItemMetadata;
		}

		static object ReadObject(string resource)
		{
			if (!string.IsNullOrEmpty(resource))
			{
				var stream = Assembly.GetManifestResourceStream(resource);

				if (stream != null)
				{
					using (var reader = new StreamReader(stream))
					{
						using (var jsonReader = new JsonTextReader(reader))
						{
							var json = JObject.Load(jsonReader);
							return json.ToObject<DynamicWrapper>();
						}
					}
				}
			}

			return null;
		}
	}
}