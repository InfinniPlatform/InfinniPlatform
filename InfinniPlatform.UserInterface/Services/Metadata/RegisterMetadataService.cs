using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными документов.
	/// </summary>
	internal sealed class RegisterMetadataService : BaseMetadataService
	{
		public RegisterMetadataService(string version, string configId, string server, int port, string route)
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
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			if (configuration.Registers.ContainsKey(item.Name))
			{
				dynamic oldRegister = configuration.Registers[item.Name];
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
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			var register = configuration.Registers[itemId];

			var registerDirectory = Path.GetDirectoryName(register.FilePath);

			if (registerDirectory != null)
			{
				Directory.Delete(registerDirectory, true);
				PackageMetadataLoader.UpdateCache();
			}
		}

		public override object GetItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			return configuration.Registers[itemId].Content;
		}

		public override IEnumerable<object> GetItems()
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			Dictionary<string, dynamic> documents = configuration.Registers;
			return documents.Values.Select(o => o.Content);
		}
	}
}