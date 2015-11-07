using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными меню.
	/// </summary>
	internal sealed class MenuMetadataService : BaseMetadataService
	{
		public MenuMetadataService(string configId)
		{
			ConfigId = configId;
		}

		public string ConfigId { get; }

		public override object CreateItem()
		{
			return new object();
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			dynamic configuration = PackageMetadataLoader.GetConfiguration(ConfigId);
			if (configuration.Menu.ContainsKey(item.Name))
			{
				dynamic oldMenu = configuration.Documents[item.Name];
				filePath = oldMenu.FilePath;
			}
			else
			{
				string directoryPath = Path.Combine(Path.GetDirectoryName(configuration.FilePath), "Menu", item.Name);
				Directory.CreateDirectory(directoryPath);

				filePath = Path.Combine(directoryPath, string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.GetConfiguration(ConfigId);
			var menu = configuration.Menu[itemId];

			var menuDirectory = Path.GetDirectoryName(menu.FilePath);

			if (menuDirectory != null)
			{
				Directory.Delete(menuDirectory, true);
				PackageMetadataLoader.UpdateCache();
			}
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.GetMenu(ConfigId, itemId);
		}

		public override IEnumerable<object> GetItems()
		{
			return PackageMetadataLoader.GetMenus(ConfigId);
		}
	}
}