using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными меню.
	/// </summary>
	internal sealed class MenuMetadataService : BaseMetadataService
	{
		public MenuMetadataService(string version, string configId, string server, int port, string route)
			: base(version, server, port, route)
		{
			_configId = configId;
		}

		private readonly string _configId;

		public string ConfigId
		{
			get { return _configId; }
		}

		public override object CreateItem()
		{
			return new DynamicWrapper();
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			//TODO Wrapper for PackageMetadataLoader.Configurations
			if (PackageMetadataLoader.Configurations[_configId].Menu.ContainsKey(item.Name))
			{
				dynamic oldMenu = PackageMetadataLoader.Configurations[_configId].Documents[item.Name];
				filePath = oldMenu.FilePath;
			}
			else
			{
				string directoryPath = Path.Combine(Path.GetDirectoryName(PackageMetadataLoader.Configurations[_configId].FilePath), "Menu", item.Name);
				Directory.CreateDirectory(directoryPath);

				filePath = Path.Combine(directoryPath, string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic menu = PackageMetadataLoader.Configurations[_configId].Menu[itemId];

			var menuDirectory = Path.GetDirectoryName(menu.FilePath);

			if (menuDirectory != null)
			{
				Directory.Delete(menuDirectory, true);
				PackageMetadataLoader.UpdateCache();
			}
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.Configurations[_configId].Menu[itemId].Content;
		}

		public override IEnumerable<object> GetItems()
		{
			Dictionary<string, dynamic> documents = PackageMetadataLoader.Configurations[_configId].Menu;
			return documents.Values.Select(o => o.Content);
		}
	}
}