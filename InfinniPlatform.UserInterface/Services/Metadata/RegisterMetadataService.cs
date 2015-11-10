using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными документов.
	/// </summary>
	internal sealed class RegisterMetadataService : BaseMetadataService
	{
		public RegisterMetadataService(string configId)
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
			dynamic oldRegister = PackageMetadataLoader.GetRegister(ConfigId, item.Name);
			if (oldRegister != null)
			{
				filePath = oldRegister.FilePath;
			}
			else
			{
				string directoryPath = Path.Combine(Path.GetDirectoryName(configuration.FilePath), "Registers", item.Name);
				Directory.CreateDirectory(directoryPath);

				filePath = Path.Combine(directoryPath, string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic register = PackageMetadataLoader.GetRegister(ConfigId, itemId);

			var registerDirectory = Path.GetDirectoryName(register.FilePath);

			if (registerDirectory != null)
			{
				Directory.Delete(registerDirectory, true);
				PackageMetadataLoader.UpdateCache();
			}
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.GetRegister(ConfigId, itemId);
		}

		public override IEnumerable<object> GetItems()
		{
			return PackageMetadataLoader.GetRegisters(ConfigId);
		}
	}
}